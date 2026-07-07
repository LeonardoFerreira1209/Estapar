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
/// Service implementation for managing Garage entities within a Park.
/// </summary>
public class GarageService(
        IGarageRepository garageRepository,
        IParkRepository parkRepository,
        IUnitOfWork unitOfWork
    ) : IGarageService
{
    /// <summary>
    /// Creates a new garage within the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park to which the garage will belong.</param>
    /// <param name="request">The request containing garage details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the created garage.</returns>
    public async Task<GarageResponse> CreateAsync(
        Guid parkId,
        CreateGarageRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var validation =
                await new CreateGarageRequestValidator()
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

            var garage = 
                await garageRepository
                .CreateAsync(
                    request.ToEntity(
                        parkId
                    )
                );

            await unitOfWork.CommitAsync();

            return garage.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a garage by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the garage.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the garage details.</returns>
    public async Task<GarageResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var garage = 
                await garageRepository
                    .GetByIdAsync(
                        id
                    )
                ?? throw GarageException.NotFound(id);

            return garage.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Retrieves all garages belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A list of garage responses.</returns>
    public async Task<List<GarageResponse>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var garages = 
                await garageRepository
                    .GetByParkIdAsync(
                        parkId,
                        cancellationToken
                    );

            return [
                .. garages.Select(
                    g => g.ToResponse()
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
    /// Updates an existing garage.
    /// </summary>
    /// <param name="id">The unique identifier of the garage to update.</param>
    /// <param name="request">The request containing updated garage details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the updated garage.</returns>
    public async Task<GarageResponse> UpdateAsync(
        UpdateGarageRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var validation =
                await new UpdateGarageRequestValidator()
                    .ValidateAsync(
                        request,
                        cancellationToken
                    );

            if (!validation.IsValid)
                await validation.GetValidationErrors();

            var garage = 
                await garageRepository
                    .GetByIdAsync(
                        request.Id
                    )
                ?? throw GarageException.NotFound(request.Id);

            await garageRepository
                .UpdateAsync(
                    garage.UpdateFromRequest(
                        request
                    )
                );

            await unitOfWork.CommitAsync();

            return garage.ToResponse();
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - Exception: {exception.Message} - {JsonConvert.SerializeObject(exception)}\n");
            throw;
        }
    }

    /// <summary>
    /// Deletes an existing garage.
    /// </summary>
    /// <param name="id">The unique identifier of the garage to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        try
        {
            await garageRepository
                .DeleteAsync(
                    await garageRepository.GetByIdAsync(
                        id
                    )
                    ?? throw GarageException.NotFound(id)
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
