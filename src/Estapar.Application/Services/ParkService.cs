using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Entities;
using Estapar.Domain.Exceptions.Base;
using Estapar.Domain.Extensions;
using FluentValidation;
using System.Net;

namespace Estapar.Application.Services;

/// <summary>
/// Service implementation for managing Park entities and their associated Lanes and Garages.
/// </summary>
public class ParkService(
    IParkRepository parkRepository,
    ILaneRepository laneRepository,
    IGarageRepository garageRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateParkRequest> createParkValidator,
    IValidator<UpdateParkRequest> updateParkValidator) : IParkService
{
    /// <inheritdoc />
    public async Task<ParkDetailResponse> CreateAsync(CreateParkRequest request)
    {
        await ValidateAsync(createParkValidator, request);

        var park = request.ToEntity();

        await parkRepository.CreateAsync(park);
        await unitOfWork.CommitAsync();

        var created = await GetParkOrThrowAsync(park.Id);

        return created.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task<ParkDetailResponse> GetByIdAsync(Guid id)
    {
        var park = await GetParkOrThrowAsync(id);

        return park.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task<List<ParkResponse>> GetAllAsync()
    {
        var parks = await parkRepository.GetAllAsync();

        return parks
            .Select(p => p.ToResponse())
            .ToList();
    }

    /// <inheritdoc />
    public async Task<ParkDetailResponse> UpdateAsync(Guid id, UpdateParkRequest request)
    {
        await ValidateAsync(updateParkValidator, request);

        var park = await GetParkOrThrowAsync(id);

        park.UpdateFromRequest(request);

        await SyncLanesAsync(park, request.Lanes);
        await SyncGaragesAsync(park, request.Garages);

        await parkRepository.UpdateAsync(park);
        await unitOfWork.CommitAsync();

        var updated = await GetParkOrThrowAsync(park.Id);

        return updated.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var park = await GetParkOrThrowAsync(id);

        await parkRepository.DeleteAsync(park);
        await unitOfWork.CommitAsync();
    }

    // --- Private helpers ---

    /// <summary>
    /// Retrieves a park with all its associated lanes and garages, or throws a <see cref="CustomException"/>
    /// with HTTP 404 when not found.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <returns>The <see cref="ParkEntity"/> with lanes and garages loaded.</returns>
    private async Task<ParkEntity> GetParkOrThrowAsync(Guid id)
    {
        var park = await parkRepository.GetWithAssociationsAsync(id);

        if (park is null)
        {
            throw new CustomException(
                HttpStatusCode.NotFound,
                "PARK_NOT_FOUND",
                null,
                [new DataNotifications($"Park with ID {id} was not found.")]
            );
        }

        return park;
    }

    /// <summary>
    /// Validates the given request using the provided validator, throwing a <see cref="CustomException"/>
    /// with HTTP 422 when validation fails.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to validate.</typeparam>
    /// <param name="validator">The FluentValidation validator instance.</param>
    /// <param name="request">The request object to validate.</param>
    private static async Task ValidateAsync<TRequest>(IValidator<TRequest> validator, TRequest request)
    {
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            var notifications = result.Errors
                .Select(e => new DataNotifications(e.ErrorMessage))
                .ToList();

            throw new CustomException(
                HttpStatusCode.UnprocessableEntity,
                "VALIDATION_ERROR",
                null,
                notifications
            );
        }
    }

    /// <summary>
    /// Synchronizes the lanes of a park with the requested lane collection.
    /// Removes lanes not present in the request, updates existing ones, and adds new ones.
    /// </summary>
    /// <param name="park">The park entity whose lanes will be synchronized.</param>
    /// <param name="requestedLanes">The desired collection of lanes from the update request.</param>
    private async Task SyncLanesAsync(ParkEntity park, List<UpdateLaneRequest> requestedLanes)
    {
        var requestedIds = requestedLanes
            .Where(l => l.Id.HasValue)
            .Select(l => l.Id!.Value)
            .ToHashSet();

        var toRemove = park.Lanes
            .Where(l => !requestedIds.Contains(l.Id))
            .ToList();

        foreach (var lane in toRemove)
        {
            await laneRepository.DeleteAsync(lane);
            park.RemoveLane(lane);
        }

        foreach (var laneRequest in requestedLanes)
        {
            if (laneRequest.Id.HasValue)
            {
                var existing = park.Lanes.FirstOrDefault(l => l.Id == laneRequest.Id.Value);
                if (existing is not null)
                {
                    existing.UpdateFromRequest(laneRequest);
                    await laneRepository.UpdateAsync(existing);
                }
            }
            else
            {
                var newLane = laneRequest.ToEntity(park.Id);
                park.AddLane(newLane);
                await laneRepository.CreateAsync(newLane);
            }
        }
    }

    /// <summary>
    /// Synchronizes the garages of a park with the requested garage collection.
    /// Removes garages not present in the request, updates existing ones, and adds new ones.
    /// </summary>
    /// <param name="park">The park entity whose garages will be synchronized.</param>
    /// <param name="requestedGarages">The desired collection of garages from the update request.</param>
    private async Task SyncGaragesAsync(ParkEntity park, List<UpdateGarageRequest> requestedGarages)
    {
        var requestedIds = requestedGarages
            .Where(g => g.Id.HasValue)
            .Select(g => g.Id!.Value)
            .ToHashSet();

        var toRemove = park.Garages
            .Where(g => !requestedIds.Contains(g.Id))
            .ToList();

        foreach (var garage in toRemove)
        {
            await garageRepository.DeleteAsync(garage);
            park.RemoveGarage(garage);
        }

        foreach (var garageRequest in requestedGarages)
        {
            if (garageRequest.Id.HasValue)
            {
                var existing = park.Garages.FirstOrDefault(g => g.Id == garageRequest.Id.Value);
                if (existing is not null)
                {
                    existing.UpdateFromRequest(garageRequest);
                    await garageRepository.UpdateAsync(existing);
                }
            }
            else
            {
                var newGarage = garageRequest.ToEntity(park.Id);
                park.AddGarage(newGarage);
                await garageRepository.CreateAsync(newGarage);
            }
        }
    }
}