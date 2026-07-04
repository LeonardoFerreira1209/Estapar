namespace Estapar.Domain.Entities.Base;

/// <summary>
/// Represents an entity with a primary key of a specified type.
/// </summary>
/// <typeparam name="TKey">The type of the primary key, which must implement <see cref="IEquatable{T}"/>.</typeparam>
public interface IEntityPrimaryKey<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public TKey Id { get; }
}
