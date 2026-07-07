using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Exceptions;
using Estapar.Domain.Extensions;
using Estapar.Domain.Extensions.Validators;
using Estapar.Domain.Validators;
using Newtonsoft.Json;
using Serilog;

namespace Estapar.Application.Services;

/// <summary>
/// Service implementation for managing Park entities and their associated Lanes and Garages.
/// </summary>
public class ParkService(
        IParkRepository parkRepository,
        ITransactionRepository transactionRepository,
        IParkedVehicleRepository parkedVehicleRepository,
        IUnitOfWork unitOfWork
    ) : IParkService
{
    /// <summary>
    /// Creates a new park with its associated lanes and garages.
    /// </summary>
    /// <param name="request">The request containing park details and nested lanes/garages.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A detailed response containing the created park with all its associations.</returns>
    public async Task<ParkDetailResponse> CreateAsync(
        CreateParkRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var validation =
                await new CreateParkRequestValidator()
                    .ValidateAsync(
                        request,
                        cancellationToken
                    );

            if (!validation.IsValid)
                await validation.GetValidationErrors();

            var response =
                await parkRepository.CreateAsync(
                    request.ToEntity()
                );

            await unitOfWork.CommitAsync();

            return response.ToDetailResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a park by its unique identifier including all associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A detailed response containing the park with all its associations.</returns>
    public async Task<ParkDetailResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var park =
                await parkRepository
                    .GetWithAssociationsAsync(
                        id,
                        cancellationToken
                    ) ?? throw ParkException.NotFound(id);

            return park.ToDetailResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves all parks with basic information (without nested collections).
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A list of park basic information.</returns>
    public async Task<List<ParkResponse>> GetAllAsync(
        CancellationToken cancellationToken
        )
    {
        try
        {
            var parks =
                await parkRepository
                    .GetAllAsync();

            return [
                .. parks.Select(
                    p => p.ToResponse()
                )
            ];
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing park and manages its associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park to update.</param>
    /// <param name="request">The request containing updated park details and nested lanes/garages.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A detailed response containing the updated park with all its associations.</returns>
    public async Task<ParkDetailResponse> UpdateAsync(
        Guid id,
        UpdateParkRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var validation =
                await new UpdateParkRequestValidator()
                    .ValidateAsync(
                        request,
                        cancellationToken
                    );

            if (!validation.IsValid)
                await validation.GetValidationErrors();

            var park =
               await parkRepository
                   .GetWithAssociationsAsync(
                       id,
                       cancellationToken
                   ) ?? throw ParkException.NotFound(id);

            park
                .UpdateFromRequest(
                    request
                );

            await parkRepository
                .UpdateAsync(
                    park
                );

            await unitOfWork.CommitAsync();

            return park.ToDetailResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Deletes a park and all its associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            await parkRepository.DeleteAsync(
                await parkRepository
                   .GetWithAssociationsAsync(
                       id,
                       cancellationToken
                   ) ?? throw ParkException.NotFound(id)
            );

            await unitOfWork.CommitAsync();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Calculates the total billing (revenue) generated by a park on a specific date.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <param name="date">The date to calculate the billing for.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the total billed amount and related billing information.</returns>
    public async Task<ParkRevenueResponse> GetRevenueByDateAsync(
        Guid id,
        DateOnly date,
        CancellationToken cancellationToken
        )
    {
        try
        {
            _ = await parkRepository.GetByIdAsync(id)
                ?? throw ParkException.NotFound(id);

            var transactions =
                await transactionRepository.GetByParkIdAndDateAsync(
                    id,
                    date,
                    cancellationToken
                );

            return transactions.ToRevenueResponse(
                id, 
                date
            );
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves every vehicle currently parked across all garages of the given park.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A list of the vehicles currently parked in the park.</returns>
    public async Task<List<ParkedVehicleResponse>> GetParkedVehiclesAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            _ = await parkRepository.GetByIdAsync(id)
                ?? throw ParkException.NotFound(id);

            var parkedVehicles =
                await parkedVehicleRepository.GetByParkIdAsync(
                    id,
                    cancellationToken
                );

            return parkedVehicles.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }
}