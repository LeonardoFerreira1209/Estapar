using Estapar.Domain.Entities.Base;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents a garage within a parking facility (park).
/// </summary>
/// <remarks>
/// <para>A garage is a subdivision of a park, providing:</para>
/// <list type="bullet">
/// <item><description><strong>Organizational Structure:</strong> Enables logical separation of parking areas within a park</description></item>
/// <item><description><strong>Capacity Management:</strong> Allows individual tracking and management of parking spaces</description></item>
/// <item><description><strong>Operational Control:</strong> Supports distinct operational policies per garage within the same park</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>Many garages belong to one park (many-to-one)</description></item>
/// </list>
/// </remarks>
public class GarageEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for the garage.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the park to which this garage belongs.
    /// </summary>
    /// <value>
    /// The unique identifier of the parent <see cref="ParkEntity"/> entity.
    /// </value>
    public Guid ParkId { get; private set; }

    /// <summary>
    /// Gets the name of the garage.
    /// </summary>
    /// <value>
    /// A descriptive name that identifies the garage within the parking facility.
    /// </value>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the timestamp when this garage was initially created in the system.
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this garage.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the park to which this garage belongs.
    /// </summary>
    /// <value>
    /// The parent <see cref="ParkEntity"/> entity that contains this garage.
    /// </value>
    public virtual ParkEntity Park { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="GarageEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    private GarageEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GarageEntity"/> class with specified park identifier and name.
    /// </summary>
    /// <param name="parkId">The identifier of the park to which this garage belongs.</param>
    /// <param name="name">The name of the garage.</param>
    public GarageEntity(Guid parkId, string name)
    {
        Id = Guid.NewGuid();
        ParkId = parkId;
        Name = name;
        Created = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the garage's information.
    /// </summary>
    /// <param name="name">The new name of the garage.</param>
    public void Update(string name)
    {
        Name = name;
        Updated = DateTime.UtcNow;
    }
}
