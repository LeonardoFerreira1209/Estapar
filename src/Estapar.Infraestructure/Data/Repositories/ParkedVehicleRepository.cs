using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="ParkedVehicleEntity"/>.
/// </summary>
/// <remarks>
/// This repository manages a high-throughput table. All write operations (insert/delete) should
/// be executed within a unit of work to ensure data consistency with the related traffic records.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class ParkedVehicleRepository(EstaparContext context)
    : GenericEntityCoreRepository<ParkedVehicleEntity>(context), IParkedVehicleRepository
{
    /// <summary>
    /// Retrieves the parked vehicle record for the specified license plate if currently inside any garage.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to search for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the <see cref="ParkedVehicleEntity"/>
    /// if the vehicle is currently parked; otherwise, <see langword="null"/>.
    /// </returns>
    public async Task<ParkedVehicleEntity> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
        ) => await context.Set<ParkedVehicleEntity>()
                .FirstOrDefaultAsync(
                    pv => pv.LicensePlate == licensePlate,
                    cancellationToken
                );

    /// <summary>
    /// Retrieves all vehicles currently parked in the specified garage.
    /// </summary>
    /// <param name="garageId">The unique identifier of the garage.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="ParkedVehicleEntity"/>
    /// records currently present in the garage, ordered by entry time (oldest first).
    /// </returns>
    public async Task<IList<ParkedVehicleEntity>> GetByGarageIdAsync(
        Guid garageId,
        CancellationToken cancellationToken = default
    ) => await context.Set<ParkedVehicleEntity>()
                .Where(pv => pv.GarageId == garageId)
                .OrderBy(pv => pv.Created)
                .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieves all vehicles currently parked across all garages of the specified park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="ParkedVehicleEntity"/>
    /// records currently present in the park, ordered by entry time (oldest first).
    /// </returns>
    public async Task<IList<ParkedVehicleEntity>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    ) => await context.Set<ParkedVehicleEntity>()
                .Where(pv => pv.Garage.ParkId == parkId)
                .OrderBy(pv => pv.Created)
                .ToListAsync(cancellationToken);

    /// <summary>
    /// Counts the number of vehicles currently parked in the specified garage.
    /// </summary>
    /// <param name="garageId">The unique identifier of the garage.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the current occupancy count for the garage.
    /// </returns>
    public async Task<int> CountByGarageIdAsync(
        Guid garageId,
        CancellationToken cancellationToken = default
    ) => await context.Set<ParkedVehicleEntity>()
            .CountAsync(
                pv => pv.GarageId == garageId,
                cancellationToken
            );

    /// <summary>
    /// Counts the number of vehicles currently parked across all garages of the specified park (sector).
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the current occupancy count for the park.
    /// </returns>
    public async Task<int> CountByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    ) => await context.Set<ParkedVehicleEntity>()
            .CountAsync(
                pv => pv.Garage.ParkId == parkId,
                cancellationToken
            );

    /// <summary>
    /// Determines whether the specified vehicle is currently parked in any garage.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <see langword="true"/>
    /// if the vehicle is currently parked; otherwise, <see langword="false"/>.
    /// </returns>
    public async Task<bool> IsParkedAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    ) => await context.Set<ParkedVehicleEntity>()
            .AnyAsync(
                pv => pv.LicensePlate == licensePlate,
                cancellationToken
            );

    /// <summary>
    /// Removes the parked vehicle record for the specified license plate (called when a vehicle exits).
    /// </summary>
    /// <remarks>
    /// This method performs a physical DELETE operation. It should be called within a unit of work
    /// along with the creation of the corresponding exit traffic record.
    /// </remarks>
    /// <param name="licensePlate">The vehicle license plate to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <see langword="true"/>
    /// if a record was found and removed; otherwise, <see langword="false"/>.
    /// </returns>
    public async Task<bool> RemoveByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
        )
    {
        var record = await context.Set<ParkedVehicleEntity>()
            .FirstOrDefaultAsync(
                pv => pv.LicensePlate == licensePlate,
                cancellationToken
            );

        if (record is null)
            return false;

        context.Set<ParkedVehicleEntity>()
            .Remove(record);

        return true;
    }
}
