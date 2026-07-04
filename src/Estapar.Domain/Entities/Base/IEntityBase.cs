namespace Estapar.Domain.Entities.Base;

/// <summary>
/// Defines the fundamental contract for all domain entities, establishing common properties for identity, 
/// audit trails, and lifecycle management across the entire system architecture.
/// </summary>
/// <remarks>
/// <para>This base interface provides essential capabilities for all domain entities:</para>
/// <list type="bullet">
/// <item><description><strong>Unique Identification:</strong> GUID-based primary key for consistent entity identification</description></item>
/// <item><description><strong>Audit Trail Support:</strong> Creation and modification timestamps for compliance and tracking</description></item>
/// <item><description><strong>Lifecycle Management:</strong> Standard patterns for entity creation, updates, and change detection</description></item>
/// <item><description><strong>ORM Integration:</strong> Compatible with Entity Framework Core and other ORM frameworks</description></item>
/// </list>
/// <para><strong>Implementation Guidelines:</strong></para>
/// <list type="bullet">
/// <item><description>All domain entities should implement this interface for consistency</description></item>
/// <item><description>Creation timestamp should be immutable after entity creation</description></item>
/// <item><description>Update timestamp should be automatically maintained by the persistence layer</description></item>
/// <item><description>GUID primary keys ensure global uniqueness and avoid identity conflicts</description></item>
/// </list>
/// <para><strong>Persistence Layer:</strong> Entity Framework configurations should automatically populate 
/// timestamps and handle concurrency controls based on these standard properties.</para>
/// </remarks>
public interface IEntityBase : IEntityPrimaryKey<Guid>
{
    /// <summary>
    /// Gets the timestamp when this entity was initially created in the system, providing the foundation for audit trails and lifecycle tracking.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> representing the exact moment when the entity was first persisted to the system. 
    /// This value is immutable and serves as the baseline for all temporal calculations and audit requirements.
    /// </value>
    /// <remarks>
    /// <para>This property serves critical functions across the system architecture:</para>
    /// <list type="bullet">
    /// <item><description><strong>Audit Compliance:</strong> Provides mandatory creation timestamps for regulatory and compliance requirements</description></item>
    /// <item><description><strong>Data Lifecycle:</strong> Enables age-based data management policies and retention strategies</description></item>
    /// <item><description><strong>Analytics:</strong> Supports temporal analysis and trend identification across entity populations</description></item>
    /// <item><description><strong>Debugging:</strong> Facilitates troubleshooting by providing clear entity creation context</description></item>
    /// <item><description><strong>Synchronization:</strong> Enables chronological ordering for replication and synchronization processes</description></item>
    /// </list>
    /// <para><strong>Implementation:</strong> This timestamp should be automatically set by the persistence layer 
    /// during entity creation and must remain immutable throughout the entity's lifecycle.</para>
    /// </remarks>
    public DateTime Created { get; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this entity, enabling change detection and incremental processing capabilities.
    /// </summary>
    /// <value>
    /// A nullable <see cref="DateTime"/> representing the last time any property of this entity was modified. 
    /// The value is null if the entity has never been updated since its initial creation.
    /// </value>
    /// <remarks>
    /// <para>This property enables sophisticated change management across the system:</para>
    /// <list type="bullet">
    /// <item><description><strong>Change Detection:</strong> Facilitates efficient identification of modified entities for incremental processing</description></item>
    /// <item><description><strong>Cache Invalidation:</strong> Supports timestamp-based cache invalidation strategies for optimal performance</description></item>
    /// <item><description><strong>Concurrency Control:</strong> Enables optimistic concurrency control and conflict resolution mechanisms</description></item>
    /// <item><description><strong>Audit Tracking:</strong> Provides temporal context for change management and compliance reporting</description></item>
    /// <item><description><strong>Synchronization:</strong> Supports delta synchronization between distributed systems and replicas</description></item>
    /// </list>
    /// <para><strong>Automation:</strong> This timestamp should be automatically updated by the persistence layer 
    /// whenever any entity property is modified, ensuring accuracy without manual intervention.</para>
    /// </remarks>
    public DateTime? Updated { get; }
}
