using Estapar.Domain.Contracts.Hubs;
using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Enums.Traffic;
using Estapar.Domain.Exceptions;
using Estapar.Domain.Exceptions.Base;
using Estapar.Domain.Extensions;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace Estapar.Application.Services;

/// <summary>
/// Processes incoming webhook events (ENTRY, PARKED, EXIT) and applies the corresponding
/// business logic: traffic registration, occupancy control, and transaction generation.
/// </summary>
public class WebhookService(
    IGarageRepository garageRepository,
    ILaneRepository laneRepository,
    IPriceTableRepository priceTableRepository,
    ITrafficRepository trafficRepository,
    IParkedVehicleRepository parkedVehicleRepository,
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork,
    ILaneHubService hubService
) : IWebhookService
{
    /// <summary>
    /// Process webhook request. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ProcessAsync(
        WebhookRequest request,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            switch (request.EventType)
            {
                case TrafficAction.Entry:
                    await ProcessEntryAsync(
                        request,
                        cancellationToken
                    );
                    break;

                case TrafficAction.Park:
                    await ProcessParkedAsync(
                        request,
                        cancellationToken
                    );
                    break;

                case TrafficAction.Exit:
                    await ProcessExitAsync(
                        request,
                        cancellationToken
                    );
                    break;

                default:
                    Log.Warning($"[WEBHOOK] Unknown event type received: {request.EventType}");
                    break;
            }
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Handles the ENTRY event:
    /// blocks the entry when the park (sector) is at 100% occupancy, otherwise calculates the
    /// dynamic entry price based on current occupancy, selects the first available garage,
    /// registers a success or error traffic and notifies hub listeners so the gate can be opened.
    /// </summary>
    private async Task ProcessEntryAsync(
        WebhookRequest request,
        CancellationToken cancellationToken
        )
    {
        var entryDate =
            request.EntryTime
            ?? DateTime.UtcNow;

        var entryLane =
            await laneRepository.GetByIdAsync(
                request.LaneId
            )
            ?? throw LaneException.NotFound(request.LaneId);

        var isAlreadyParked =
            await parkedVehicleRepository.IsParkedAsync(
                request.LicensePlate,
                cancellationToken
            );

        if (isAlreadyParked)
        {
            await CreateErrorTrafficAsync(
                request,
                entryDate,
                entryLane.Id,
                TrafficAction.Entry,
                TrafficError.VehicleAlreadyInside
            );

            return;
        }

        var totalCapacity =
            await garageRepository.CountByParkIdAsync(
                entryLane.ParkId,
                cancellationToken
            );

        var occupiedSpots =
            await parkedVehicleRepository.CountByParkIdAsync(
                entryLane.ParkId,
                cancellationToken
            );

        if (occupiedSpots >= totalCapacity)
        {
            await CreateErrorTrafficAsync(
                request,
                entryDate,
                entryLane.Id,
                TrafficAction.Entry,
                TrafficError.GarageFull
            );

            return;
        }

        var garage =
            await garageRepository.GetFirstAvailableAsync(
                entryLane.ParkId,
                cancellationToken
            );

        if (garage is null)
        {
            await CreateErrorTrafficAsync(
                request,
                entryDate,
                entryLane.Id,
                TrafficAction.Entry,
                TrafficError.GarageFull
            );

            return;
        }

        var priceTable =
            await priceTableRepository.GetByParkIdAsync(
                entryLane.ParkId,
                cancellationToken
            );

        var entryPrice = 
            priceTable.CalculateEntryPrice(
                occupiedSpots, 
                totalCapacity
            );

        var traffic =
            request.ToTrafficEntity(
                entryDate,
                entryLane.Id,
                TrafficAction.Entry,
                success: true,
                balance: entryPrice
            );

        await trafficRepository.CreateAsync(traffic);
        await unitOfWork.CommitAsync();

        await hubService.NotifyVehicleArrivalAsync(
            entryLane,
            request.LicensePlate,
            cancellationToken
        );
    }

    /// <summary>
    /// Handles the PARKED event:
    /// retrieves the open entry traffic, selects the available garage, creates a park traffic
    /// and registers the vehicle as parked, linking it to the original entry traffic.
    /// </summary>
    private async Task ProcessParkedAsync(
        WebhookRequest request,
        CancellationToken cancellationToken
        )
    {
        var parkedDate =
            request.EntryTime
            ?? DateTime.UtcNow;

        var entryLane =
            await laneRepository
                .GetByIdAsync(
                    request.LaneId
                )
            ?? throw LaneException.NotFound(
                request.LaneId
            );

        var entryTraffic =
            await trafficRepository.GetLastOpenEntryAsync(
                request.LicensePlate,
                cancellationToken
            ) ?? throw new CustomException(
                HttpStatusCode.NotFound,
                "PARKED received but no open entry traffic found for vehicle",
                null
            );

        var garage =
            await garageRepository.GetFirstAvailableAsync(
                entryLane.ParkId,
                cancellationToken
            );

        if (garage is null)
        {
            await CreateErrorTrafficAsync(
                request,
                parkedDate,
                entryLane.Id,
                TrafficAction.Park,
                TrafficError.GarageFull
            );

            return;
        }

        var traffic =
            request.ToTrafficEntity(
                parkedDate,
                entryLane.Id,
                TrafficAction.Park,
                success: true
            );

        await trafficRepository.CreateAsync(traffic);

        var parkedVehicle = request.ToParkedVehicleEntity(
            entryTraffic.Id,
            garage.Id
        );

        await parkedVehicleRepository.CreateAsync(parkedVehicle);

        await unitOfWork.CommitAsync();
    }

    /// <summary>
    /// Handles the EXIT event:
    /// registers exit traffic, calculates the transaction charge, removes the parked vehicle record.
    /// </summary>
    private async Task ProcessExitAsync(
        WebhookRequest request,
        CancellationToken cancellationToken
        )
    {
        var exitDate =
            request.ExitTime
            ?? DateTime.UtcNow;

        var parkedVehicle =
            await parkedVehicleRepository.GetByLicensePlateAsync(
                request.LicensePlate,
                cancellationToken
            );

        if (parkedVehicle is null)
        {
            await CreateErrorTrafficAsync(
                request, 
                exitDate, 
                request.LaneId, 
                TrafficAction.Exit, 
                TrafficError.VehicleNotInside
            );

            return;
        }

        var garage = 
            await garageRepository.GetByIdAsync(
                parkedVehicle.GarageId
            );

        var priceTable = 
            await priceTableRepository.GetByParkIdAsync(
                garage.ParkId, 
                cancellationToken
            );

        var entryTraffic = 
            await trafficRepository.GetByIdAsync(
                parkedVehicle.EntryTrafficId
            );

        var exitTraffic = 
            request.ToTrafficEntity(
                exitDate, 
                request.LaneId, 
                TrafficAction.Exit, 
                success: true
            );

        await trafficRepository.CreateAsync(exitTraffic);

        var transaction = 
            entryTraffic.ToTransactionEntity(
                exitTraffic, 
                priceTable
            );

        await transactionRepository.CreateAsync(transaction);

        await parkedVehicleRepository.RemoveByLicensePlateAsync(
            request.LicensePlate, 
            cancellationToken
        );

        await unitOfWork.CommitAsync();
    }

    /// <summary>
    /// Creates and persists an unsuccessful traffic record, committing the change immediately.
    /// </summary>
    /// <param name="request">The webhook request containing the vehicle license plate.</param>
    /// <param name="date">The date and time of the failed attempt.</param>
    /// <param name="laneId">The identifier of the lane where the attempt occurred.</param>
    /// <param name="action">The action type (entry, park, or exit).</param>
    /// <param name="error">The error that caused the attempt to fail.</param>
    private async Task CreateErrorTrafficAsync(
        WebhookRequest request,
        DateTime date,
        Guid laneId,
        TrafficAction action,
        TrafficError error
        )
    {
        var traffic =
            request.ToTrafficEntity(
                date,
                laneId,
                action,
                success: false,
                error
            );

        await trafficRepository.CreateAsync(traffic);
        await unitOfWork.CommitAsync();
    }
}
