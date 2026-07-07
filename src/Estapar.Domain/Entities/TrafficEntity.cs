using Estapar.Domain.Entities.Base;
using Estapar.Domain.Enums.Traffic;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents a traffic attempt log at a parking lane (entry or exit).
/// </summary>
/// <remarks>
/// <para>A traffic record is generated every time a vehicle approaches the gate of a parking facility.
/// It logs the outcome of the attempt, including any errors that may have occurred:</para>
/// <list type="bullet">
/// <item><description><strong>Entry attempt:</strong> Vehicle trying to enter but already registered as inside</description></item>
/// <item><description><strong>Exit attempt:</strong> Vehicle trying to exit but not registered as inside</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>Many traffic records belong to one lane (many-to-one)</description></item>
/// </list>
/// </remarks>
public class TrafficEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for this traffic record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the vehicle license plate associated with this traffic attempt.
    /// </summary>
    public string LicensePlate { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date and time of this traffic attempt.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Gets the identifier of the lane where this traffic attempt occurred.
    /// </summary>
    public Guid LaneId { get; private set; }

    /// <summary>
    /// Gets the error code associated with this traffic attempt, if any.
    /// </summary>
    /// <value>
    /// A <see cref="TrafficError"/> value describing the error that occurred, or <see cref="TrafficError.None"/> if the attempt was successful.
    /// </value>
    public TrafficError Error { get; private set; }

    /// <summary>
    /// Gets the action type of this traffic attempt.
    /// </summary>
    /// <value>
    /// A <see cref="TrafficAction"/> value indicating whether this was an entry or exit attempt.
    /// </value>
    public TrafficAction Action { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this traffic attempt was successful.
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// Gets the price associated with this traffic attempt.
    /// </summary>
    /// <value>
    /// For a successful entry attempt, this is the dynamic hourly price locked in at the time of
    /// entry (based on the park's occupancy at that moment). For park/exit/error attempts, this is zero.
    /// </value>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Gets the timestamp when this traffic record was created in the system.
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this traffic record.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the lane where this traffic attempt occurred.
    /// </summary>
    public virtual LaneEntity Lane { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrafficEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    protected TrafficEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrafficEntity"/> class representing a traffic attempt.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="date">The date and time of the attempt.</param>
    /// <param name="laneId">The identifier of the lane where the attempt occurred.</param>
    /// <param name="action">The action type (entry or exit).</param>
    /// <param name="success">Whether the attempt was successful.</param>
    /// <param name="balance">The dynamic entry price locked in for this attempt, or zero if not applicable.</param>
    /// <param name="error">The error code, if any occurred during the attempt.</param>
    public TrafficEntity(
        string licensePlate,
        DateTime date,
        Guid laneId,
        TrafficAction action,
        bool success,
        decimal balance,
        TrafficError error = TrafficError.None
        )
    {
        Id = Guid.NewGuid();
        LicensePlate = licensePlate;
        Date = date;
        LaneId = laneId;
        Action = action;
        Success = success;
        Balance = balance;
        Error = error;
        Created = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks this traffic attempt as failed with the specified error.
    /// </summary>
    /// <param name="error">The error code describing why the attempt failed.</param>
    public void MarkAsFailed(TrafficError error)
    {
        Success = false;
        Error = error;
        Updated = DateTime.UtcNow;
    }
}
