using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="LaneEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class LaneRepository(EstaparContext context)
    : GenericEntityCoreRepository<LaneEntity>(context), ILaneRepository
{
    /// <summary>
    /// Retrieves all lanes that belong to the specified parking facility.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="LaneEntity"/>
    /// records belonging to the specified park.
    /// </returns>
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
