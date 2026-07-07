using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Traffic;

namespace Estapar.Domain.Contracts.Repositories;

/// <summary>
/// Defines the repository contract for performing data access operations on <see cref="TrafficEntity"/>.
/// </summary>
public interface ITrafficRepository : IGenerictEntityCoreRepository<TrafficEntity>
{
    /// <summary>
    /// Asynchronously retrieves the most recent successful entry traffic record for a given vehicle
    /// that does not yet have a corresponding exit record (i.e., the vehicle is still inside).
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the latest open entry
    /// <see cref="TrafficEntity"/>, or <see langword="null"/> if not found.
    /// </returns>
    Task<TrafficEntity> GetLastOpenEntryAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously determines whether the specified vehicle is currently registered as inside the parking facility.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <see langword="true"/> if the vehicle
    /// is currently inside; otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> IsVehicleInsideAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously retrieves all traffic records for a given vehicle license plate.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="TrafficEntity"/>
    /// records ordered by date descending.
    /// </returns>
    Task<IList<TrafficEntity>> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );
}
