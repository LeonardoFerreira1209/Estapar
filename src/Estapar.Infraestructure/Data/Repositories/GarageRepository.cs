using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="GarageEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class GarageRepository(EstaparContext context)
    : GenericEntityCoreRepository<GarageEntity>(context), IGarageRepository
{
    /// <summary>
    /// Retrieves all garages that belong to the specified parking facility.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="GarageEntity"/>
    /// records belonging to the specified park.
    /// </returns>
    public async Task<IList<GarageEntity>> GetByParkIdAsync(
        Guid parkId, 
        CancellationToken cancellationToken = default
    ) => await context.Set<GarageEntity>()
            .Where(g => g.ParkId == parkId)
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieves the first garage belonging to the specified park that currently has no parked vehicles.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector) to search within.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the first available
    /// <see cref="GarageEntity"/> within the park, or <see langword="null"/> if all of the park's garages are occupied.
    /// </returns>
    public async Task<GarageEntity> GetFirstAvailableAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    ) => await context.Set<GarageEntity>()
            .Where(g => g.ParkId == parkId)
            .Where(g => !context.Set<ParkedVehicleEntity>().Any(pv => pv.GarageId == g.Id))
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Counts the total number of garages (spots) belonging to the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the total spot capacity for the park.
    /// </returns>
    public async Task<int> CountByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    ) => await context.Set<GarageEntity>()
            .CountAsync(
                g => g.ParkId == parkId, 
                cancellationToken
            );
}
