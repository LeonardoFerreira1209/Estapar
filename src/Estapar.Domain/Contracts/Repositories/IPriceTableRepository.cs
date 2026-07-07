using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Contracts.Repositories;

/// <summary>
/// Defines the repository contract for performing data access operations on <see cref="PriceTableEntity"/>.
/// </summary>
public interface IPriceTableRepository : IGenerictEntityCoreRepository<PriceTableEntity>
{
    /// <summary>
    /// Asynchronously retrieves the price table associated with the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="PriceTableEntity"/>
    /// for the given park, or <see langword="null"/> if not found.
    /// </returns>
    Task<PriceTableEntity> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    );
}
