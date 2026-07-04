using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="ParkEntity"/>, including loading associated lanes and garages.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class ParkRepository(DbContext context)
    : GenericEntityCoreRepository<ParkEntity>(context), IParkRepository
{
    /// <inheritdoc />
    public async Task<ParkEntity> GetWithAssociationsAsync(
        Guid id, 
        CancellationToken cancellationToken = default
        )
    {
        return await context.Set<ParkEntity>()
            .Include(p => p.Lanes)
            .Include(p => p.Garages)
            .FirstOrDefaultAsync(
                p => p.Id == id, 
                cancellationToken
            );
    }
}
