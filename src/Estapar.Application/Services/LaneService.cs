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
/// Service implementation for managing Lane entities within a Park.
/// </summary>
public class LaneService(
        ILaneRepository laneRepository,
        IParkRepository parkRepository,
        IUnitOfWork unitOfWork
    ) : ILaneService
{
    /// <summary>
    /// Creates a new lane within the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park to which the lane will belong.</param>
    /// <param name="request">The request containing lane details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the created lane.</returns>
    public async Task<LaneResponse> CreateAsync(
        Guid parkId,
        CreateLaneRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var validation =
                await new CreateLaneRequestValidator()
                    .ValidateAsync(
                        request,
                        cancellationToken
                    );

            if (!validation.IsValid)
                await validation.GetValidationErrors();

            _ = 
                await parkRepository
                    .GetByIdAsync(
                        parkId
                    )
                ?? throw ParkException.NotFound(parkId);

            var lane = 
                await laneRepository
                .CreateAsync(
                    request.ToEntity(
                        parkId
                    )
                );

            await unitOfWork.CommitAsync();

            return lane.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a lane by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the lane.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the lane details.</returns>
    public async Task<LaneResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var lane = 
                await laneRepository
                .GetByIdAsync(
                    id
                )
                ?? throw LaneException.NotFound(id);

            return lane.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves all lanes belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A list of lane responses.</returns>
    public async Task<List<LaneResponse>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var lanes = 
                await laneRepository
                    .GetByParkIdAsync(
                        parkId,
                        cancellationToken
                    );

            return [
                .. lanes.Select(
                    l => l.ToResponse()
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
    /// Updates an existing lane.
    /// </summary>
    /// <param name="request">The request containing updated lane details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the updated lane.</returns>
    public async Task<LaneResponse> UpdateAsync(
        UpdateLaneRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var validation =
                await new UpdateLaneRequestValidator()
                    .ValidateAsync(
                        request,
                        cancellationToken
                    );

            if (!validation.IsValid)
                await validation.GetValidationErrors();

            var lane = 
                await laneRepository
                    .GetByIdAsync(
                        request.Id
                    )
                ?? throw LaneException.NotFound(request.Id);

            await laneRepository.UpdateAsync(
                lane.UpdateFromRequest(
                    request
                )
            );

            await unitOfWork.CommitAsync();

            return lane.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Deletes an existing lane.
    /// </summary>
    /// <param name="id">The unique identifier of the lane to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var lane =
                await laneRepository
                    .GetByIdAsync(
                        id
                    )
                    ?? throw LaneException.NotFound(id);

            await laneRepository
                .DeleteAsync(
                    lane
                );

            await unitOfWork.CommitAsync();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }
}
