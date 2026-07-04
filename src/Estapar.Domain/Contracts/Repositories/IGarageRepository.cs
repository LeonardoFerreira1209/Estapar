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
}
