using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="GarageEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class GarageRepository(DbContext context)
    : GenericEntityCoreRepository<GarageEntity>(context), IGarageRepository
{
    /// <inheritdoc />
    public async Task<IList<GarageEntity>> GetByParkIdAsync(
        Guid parkId, 
        CancellationToken cancellationToken = default
        )
    {
        return await context.Set<GarageEntity>()
            .Where(g => g.ParkId == parkId)
            .ToListAsync(cancellationToken);
    }
}
