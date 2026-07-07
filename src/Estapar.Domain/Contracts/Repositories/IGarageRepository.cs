using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Contracts.Repositories;

/// <summary>
/// Defines the repository contract for performing data access operations on <see cref="GarageEntity"/>.
/// </summary>
/// <remarks>
/// Extends <see cref="IGenerictEntityCoreRepository{T}"/> with garage-specific query operations.
/// </remarks>
public interface IGarageRepository : IGenerictEntityCoreRepository<GarageEntity>
{
    /// <summary>
    /// Asynchronously retrieves all garages belonging to a specific park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of
    /// <see cref="GarageEntity"/> instances associated with the specified park.
    /// </returns>
    Task<IList<GarageEntity>> GetByParkIdAsync(
        Guid parkId, 
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously retrieves the first garage belonging to the specified park that currently has no parked vehicles.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector) to search within.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the first available
    /// <see cref="GarageEntity"/> within the park, or <see langword="null"/> if all of the park's garages are occupied.
    /// </returns>
    Task<GarageEntity?> GetFirstAvailableAsync(Guid parkId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously counts the total number of garages (spots) belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the total spot
    /// capacity for the park.
    /// </returns>
    Task<int> CountByParkIdAsync(Guid parkId, CancellationToken cancellationToken = default);
}
