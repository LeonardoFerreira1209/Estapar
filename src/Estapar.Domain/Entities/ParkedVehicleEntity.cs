using Estapar.Domain.Entities.Base;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents a vehicle currently parked inside a garage.
/// </summary>
/// <remarks>
/// <para>This is a high-throughput control table. Records are inserted when a vehicle enters
/// and permanently deleted when the vehicle exits. At any point in time, the records present
/// in this table represent the exact set of vehicles inside each garage.</para>
/// <list type="bullet">
/// <item><description><strong>Occupancy Control:</strong> Used to determine whether a garage is occupied and to validate entry/exit attempts</description></item>
/// <item><description><strong>Duplicate Entry Prevention:</strong> Enforces that the same vehicle cannot be registered twice in the same garage</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>Many parked vehicles belong to one garage (many-to-one)</description></item>
/// <item><description>One parked vehicle references one entry traffic record (one-to-one)</description></item>
/// </list>
/// </remarks>
public class ParkedVehicleEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for this parked vehicle record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the vehicle license plate.
    /// </summary>
    public string LicensePlate { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the identifier of the entry traffic record that originated this parking session.
    /// </summary>
    public Guid EntryTrafficId { get; private set; }

    /// <summary>
    /// Gets the identifier of the garage where the vehicle is currently parked.
    /// </summary>
    public Guid GarageId { get; private set; }

    /// <summary>
    /// Gets the timestamp when this parked vehicle record was created (i.e., when the vehicle entered).
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this record.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the entry traffic record associated with this parking session.
    /// </summary>
    public virtual TrafficEntity EntryTraffic { get; private set; } = null!;

    /// <summary>
    /// Gets the garage where the vehicle is currently parked.
    /// </summary>
    public virtual GarageEntity Garage { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParkedVehicleEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    protected ParkedVehicleEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParkedVehicleEntity"/> class,
    /// registering a vehicle as parked in a specific garage.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="entryTrafficId">The identifier of the entry traffic record.</param>
    /// <param name="garageId">The identifier of the garage where the vehicle entered.</param>
    public ParkedVehicleEntity(
        string licensePlate,
        Guid entryTrafficId,
        Guid garageId
        )
    {
        Id = Guid.NewGuid();
        LicensePlate = licensePlate;
        EntryTrafficId = entryTrafficId;
        GarageId = garageId;
        Created = DateTime.UtcNow;
    }
}
