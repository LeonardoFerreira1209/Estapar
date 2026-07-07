using Estapar.Domain.Entities.Base;
using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents a lane in a parking facility (park) for managing vehicle entry and exit flow.
/// </summary>
/// <remarks>
/// <para>A lane controls the traffic flow in a park by:</para>
/// <list type="bullet">
/// <item><description><strong>Traffic Direction:</strong> Designates whether vehicles enter or exit through the lane</description></item>
/// <item><description><strong>Operational Control:</strong> Manages lane availability and operational status</description></item>
/// <item><description><strong>Flow Management:</strong> Enables efficient vehicle routing and congestion control</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>Many lanes belong to one park (many-to-one)</description></item>
/// </list>
/// </remarks>
public class LaneEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for the lane.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the park to which this lane belongs.
    /// </summary>
    /// <value>
    /// The unique identifier of the parent <see cref="ParkEntity"/> entity.
    /// </value>
    public Guid ParkId { get; private set; }

    /// <summary>
    /// Gets the name of the lane.
    /// </summary>
    /// <value>
    /// A descriptive name that identifies the lane within the parking facility.
    /// </value>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the type of the lane.
    /// </summary>
    /// <value>
    /// A <see cref="LaneType"/> value indicating whether this is an entry or exit lane.
    /// </value>
    public LaneType LaneType { get; private set; }

    /// <summary>
    /// Gets the operational status of the lane.
    /// </summary>
    /// <value>
    /// A <see cref="Status"/> value indicating whether the lane is currently active or inactive.
    /// </value>
    public LaneStatus Status { get; private set; }

    /// <summary>
    /// Gets the timestamp when this lane was initially created in the system.
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this lane.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the park to which this lane belongs.
    /// </summary>
    /// <value>
    /// The parent <see cref="ParkEntity"/> entity that contains this lane.
    /// </value>
    public virtual ParkEntity Park { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="LaneEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    protected LaneEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LaneEntity"/> class with specified properties.
    /// </summary>
    /// <param name="parkId">The identifier of the park to which this lane belongs.</param>
    /// <param name="name">The name of the lane.</param>
    /// <param name="laneType">The type of the lane (Entry or Exit).</param>
    /// <param name="status">The operational status of the lane.</param>
    public LaneEntity(
        Guid parkId, 
        string name, 
        LaneType laneType, 
        LaneStatus status
        )
    {
        Id = Guid.NewGuid();
        ParkId = parkId;
        Name = name;
        LaneType = laneType;
        Status = status;
        Created = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the lane's information.
    /// </summary>
    /// <param name="name">The new name of the lane.</param>
    /// <param name="laneType">The new type of the lane.</param>
    /// <param name="status">The new operational status of the lane.</param>
    public void Update(
        string name, 
        LaneType laneType, 
        LaneStatus status
        )
    {
        Name = name;
        LaneType = laneType;
        Status = status;
        Updated = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the lane's operational status.
    /// </summary>
    /// <param name="status">The new operational status of the lane.</param>
    public void UpdateStatus(LaneStatus status)
    {
        Status = status;
        Updated = DateTime.UtcNow;
    }
}
