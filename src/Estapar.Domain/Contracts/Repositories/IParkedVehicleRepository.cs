using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Contracts.Repositories;

/// <summary>
/// Defines the repository contract for performing data access operations on <see cref="ParkedVehicleEntity"/>.
/// </summary>
/// <remarks>
/// This is a high-throughput repository. Records are inserted on vehicle entry and deleted on vehicle exit.
/// </remarks>
public interface IParkedVehicleRepository : IGenerictEntityCoreRepository<ParkedVehicleEntity>
{
    /// <summary>
    /// Asynchronously retrieves the parked vehicle record for the given license plate, if the vehicle is currently inside.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to search for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the <see cref="ParkedVehicleEntity"/>
    /// if the vehicle is currently parked; otherwise, <see langword="null"/>.
    /// </returns>
    Task<ParkedVehicleEntity> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously retrieves all parked vehicle records for the given garage.
    /// </summary>
    /// <param name="garageId">The unique identifier of the garage.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the list of
    /// <see cref="ParkedVehicleEntity"/> records currently present in the garage.
    /// </returns>
    Task<IList<ParkedVehicleEntity>> GetByGarageIdAsync(
        Guid garageId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously retrieves all vehicles currently parked across all garages of the given park.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the list of
    /// <see cref="ParkedVehicleEntity"/> records currently present in the park, ordered by entry time (oldest first).
    /// </returns>
    Task<IList<ParkedVehicleEntity>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously counts the number of vehicles currently parked in the given garage.
    /// </summary>
    /// <param name="garageId">The unique identifier of the garage.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the current occupancy count.
    /// </returns>
    Task<int> CountByGarageIdAsync(
        Guid garageId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously counts the number of vehicles currently parked across all garages of the given park (sector).
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the current occupancy count for the park.
    /// </returns>
    Task<int> CountByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously determines whether the specified vehicle is currently parked in any garage.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <see langword="true"/>
    /// if the vehicle is currently parked; otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> IsParkedAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously removes the parked vehicle record by license plate (called on vehicle exit).
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <see langword="true"/>
    /// if a record was found and removed; otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> RemoveByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );
}
