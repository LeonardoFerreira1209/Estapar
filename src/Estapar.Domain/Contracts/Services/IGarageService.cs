using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;

namespace Estapar.Domain.Contracts.Services;

/// <summary>
/// Service interface for managing Garage entities within a Park.
/// </summary>
public interface IGarageService
{
    /// <summary>
    /// Creates a new garage within the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park to which the garage will belong.</param>
    /// <param name="request">The request containing garage details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the created garage.</returns>
    Task<GarageResponse> CreateAsync(
        Guid parkId,
        CreateGarageRequest request,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Retrieves a garage by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the garage.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the garage details.</returns>
    Task<GarageResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Retrieves all garages belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A list of garage responses.</returns>
    Task<List<GarageResponse>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Updates an existing garage.
    /// </summary>
    /// <param name="request">The request containing updated garage details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the updated garage.</returns>
    Task<GarageResponse> UpdateAsync(
        UpdateGarageRequest request,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Deletes an existing garage.
    /// </summary>
    /// <param name="id">The unique identifier of the garage to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    );
}
