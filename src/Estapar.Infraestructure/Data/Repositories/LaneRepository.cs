using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="LaneEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class LaneRepository(DbContext context)
    : GenericEntityCoreRepository<LaneEntity>(context), ILaneRepository
{
    /// <inheritdoc />
    public async Task<IList<LaneEntity>> GetByParkIdAsync(
        Guid parkId, 
        CancellationToken cancellationToken = default
        )
    {
        return await context.Set<LaneEntity>()
            .Where(l => l.ParkId == parkId)
            .ToListAsync(cancellationToken);
    }
}
