using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;

namespace Estapar.Domain.Contracts.Services;

/// <summary>
/// Service interface for managing Park entities and their associated Lanes and Garages.
/// </summary>
public interface IParkService
{
    /// <summary>
    /// Creates a new park with its associated lanes and garages.
    /// </summary>
    /// <param name="request">The request containing park details and nested lanes/garages.</param>
    /// <returns>A detailed response containing the created park with all its associations.</returns>
    Task<ParkDetailResponse> CreateAsync(CreateParkRequest request);

    /// <summary>
    /// Retrieves a park by its unique identifier including all associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <returns>A detailed response containing the park with all its associations.</returns>
    Task<ParkDetailResponse> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all parks with basic information (without nested collections).
    /// </summary>
    /// <returns>A list of park basic information.</returns>
    Task<List<ParkResponse>> GetAllAsync();

    /// <summary>
    /// Updates an existing park and manages its associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park to update.</param>
    /// <param name="request">The request containing updated park details and nested lanes/garages.</param>
    /// <returns>A detailed response containing the updated park with all its associations.</returns>
    Task<ParkDetailResponse> UpdateAsync(
        Guid id, 
        UpdateParkRequest request
    );

    /// <summary>
    /// Deletes a park and all its associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}
