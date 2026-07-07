using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Traffic;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="TrafficEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class TrafficRepository(EstaparContext context)
    : GenericEntityCoreRepository<TrafficEntity>(context), ITrafficRepository
{
    /// <summary>
    /// Retrieves the most recent successful entry traffic record for a vehicle that hasn't exited yet.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to search for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the most recent open entry
    /// <see cref="TrafficEntity"/> for the given vehicle, or <see langword="null"/> if no open entry is found.
    /// </returns>
    public async Task<TrafficEntity> GetLastOpenEntryAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    ) => await context.Set<TrafficEntity>()
        .Where(t =>
            t.LicensePlate == licensePlate &&
            t.Action == TrafficAction.Entry &&
            t.Success
        )
        .OrderByDescending(t => t.Date)
        .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Determines whether the specified vehicle is currently registered as inside the parking facility.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <see langword="true"/> if the vehicle
    /// has an open entry record (i.e., is currently inside); otherwise, <see langword="false"/>.
    /// </returns>
    public async Task<bool> IsVehicleInsideAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    ) => await GetLastOpenEntryAsync(
            licensePlate, 
            cancellationToken
    ) is not null;

    /// <summary>
    /// Retrieves all traffic records (both entry and exit attempts) for the specified vehicle license plate.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to search for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="TrafficEntity"/>
    /// records for the given vehicle, ordered by date descending (most recent first).
    /// </returns>
    public async Task<IList<TrafficEntity>> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    ) => await context.Set<TrafficEntity>()
            .Where(t => t.LicensePlate == licensePlate)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
}
