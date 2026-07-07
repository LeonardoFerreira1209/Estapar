using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="PriceTableEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class PriceTableRepository(EstaparContext context)
    : GenericEntityCoreRepository<PriceTableEntity>(context), IPriceTableRepository
{
    /// <summary>
    /// Retrieves the price table configuration for the specified parking facility.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the <see cref="PriceTableEntity"/>
    /// for the given park, or <see langword="null"/> if no price table is configured.
    /// </returns>
    public async Task<PriceTableEntity> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    ) =>await context.Set<PriceTableEntity>()
        .FirstOrDefaultAsync(
            pt => pt.ParkId == parkId,
            cancellationToken
        );
}
