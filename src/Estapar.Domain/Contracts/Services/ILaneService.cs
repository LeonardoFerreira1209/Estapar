using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;

namespace Estapar.Domain.Contracts.Services;

/// <summary>
/// Service interface for managing Lane entities within a Park.
/// </summary>
public interface ILaneService
{
    /// <summary>
    /// Creates a new lane within the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park to which the lane will belong.</param>
    /// <param name="request">The request containing lane details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the created lane.</returns>
    Task<LaneResponse> CreateAsync(
        Guid parkId,
        CreateLaneRequest request,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Retrieves a lane by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the lane.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the lane details.</returns>
    Task<LaneResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Retrieves all lanes belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A list of lane responses.</returns>
    Task<List<LaneResponse>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Updates an existing lane.
    /// </summary>
    /// <param name="request">The request containing updated lane details.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A response containing the updated lane.</returns>
    Task<LaneResponse> UpdateAsync(
        UpdateLaneRequest request,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Deletes an existing lane.
    /// </summary>
    /// <param name="id">The unique identifier of the lane to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    );
}
