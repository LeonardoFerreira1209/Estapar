using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Contracts.Repositories;

/// <summary>
/// Defines the repository contract for performing data access operations on <see cref="ParkEntity"/>.
/// </summary>
/// <remarks>
/// Extends <see cref="IGenerictEntityCoreRepository{T}"/> with park-specific query operations,
/// including loading of associated <see cref="LaneEntity"/> and <see cref="GarageEntity"/> collections.
/// </remarks>
public interface IParkRepository : IGenerictEntityCoreRepository<ParkEntity>
{
    /// <summary>
    /// Asynchronously retrieves a park by its identifier, including its associated lanes and garages.
    /// </summary>
    /// <param name="id">The unique identifier of the park.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="ParkEntity"/>
    /// with its lanes and garages loaded, or <see langword="null"/> if not found.
    /// </returns>
    Task<ParkEntity> GetWithAssociationsAsync(
        Guid id, 
        CancellationToken cancellationToken = default
    );
}
