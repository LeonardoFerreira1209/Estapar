using Estapar.Domain.Entities.Base;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents a parking facility that contains multiple garages and lanes.
/// </summary>
/// <remarks>
/// <para>A park is the top-level entity in the parking management hierarchy, serving as:</para>
/// <list type="bullet">
/// <item><description><strong>Physical Location:</strong> Represents a distinct parking facility with its own identity and characteristics</description></item>
/// <item><description><strong>Container:</strong> Aggregates multiple garages and lanes under a single management unit</description></item>
/// <item><description><strong>Business Unit:</strong> Serves as the primary organizational unit for operations and reporting</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>One park contains many garages (one-to-many)</description></item>
/// <item><description>One park contains many lanes (one-to-many)</description></item>
/// </list>
/// </remarks>
public class ParkEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for the park.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the park.
    /// </summary>
    /// <value>
    /// A descriptive name that uniquely identifies the parking facility for users and administrators.
    /// </value>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the description of the park.
    /// </summary>
    /// <value>
    /// A detailed description providing additional information about the park's location, features, or characteristics.
    /// </value>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the timestamp when this park was initially created in the system.
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this park.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the collection of garages associated with this park.
    /// </summary>
    /// <value>
    /// A collection of <see cref="GarageEntity"/> entities that belong to this park.
    /// </value>
    public virtual ICollection<GarageEntity> Garages { get; private set; } = [];

    /// <summary>
    /// Gets the collection of lanes associated with this park.
    /// </summary>
    /// <value>
    /// A collection of <see cref="LaneEntity"/> entities that belong to this park.
    /// </value>
    public virtual ICollection<LaneEntity> Lanes { get; private set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ParkEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    private ParkEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParkEntity"/> class with the specified attributes.
    /// </summary>
    /// <param name="id">The unique identifier for the park.</param>
    /// <param name="name">The name of the park.</param>
    /// <param name="description">The description of the park.</param>
    /// <param name="lanes">The initial collection of lanes associated with this park.</param>
    /// <param name="garages">The initial collection of garages associated with this park.</param>
    public ParkEntity(
        Guid id, 
        string name, 
        string description, 
        ICollection<LaneEntity> lanes, 
        ICollection<GarageEntity> garages
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Created = DateTime.UtcNow;
        Lanes = lanes;
        Garages = garages;
    }

    /// <summary>
    /// Updates the park's information.
    /// </summary>
    /// <param name="name">The new name of the park.</param>
    /// <param name="description">The new description of the park.</param>
    public void Update(
        string name, 
        string description
        )
    {
        Name = name;
        Description = description;
        Updated = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a lane to this park's collection for cascade persistence.
    /// </summary>
    /// <param name="lane">The lane entity to associate with this park.</param>
    public void AddLane(LaneEntity lane)
    {
        Lanes.Add(lane);
    }

    /// <summary>
    /// Adds a garage to this park's collection for cascade persistence.
    /// </summary>
    /// <param name="garage">The garage entity to associate with this park.</param>
    public void AddGarage(GarageEntity garage)
    {
        Garages.Add(garage);
    }

    /// <summary>
    /// Removes a lane from this park's collection.
    /// </summary>
    /// <param name="lane">The lane entity to remove.</param>
    public void RemoveLane(LaneEntity lane)
    {
        Lanes.Remove(lane);
    }

    /// <summary>
    /// Removes a garage from this park's collection.
    /// </summary>
    /// <param name="garage">The garage entity to remove.</param>
    public void RemoveGarage(GarageEntity garage)
    {
        Garages.Remove(garage);
    }
}
