using Estapar.Domain.Entities;

namespace Estapar.Domain.Builders;

/// <summary>
/// Provides a fluent interface for building and configuring a garage entity.
/// </summary>
public sealed class GarageEntityBuilder
{
    private Guid parkId;
    private string name;

    /// <summary>
    /// Sets the park identifier for the garage being built.
    /// </summary>
    /// <param name="parkId">The unique identifier of the parent park.</param>
    /// <returns>The current <see cref="GarageEntityBuilder"/> instance, allowing for method chaining.</returns>
    public GarageEntityBuilder AddParkId(Guid parkId)
    {
        this.parkId = parkId;

        return this;
    }

    /// <summary>
    /// Sets the name for the garage being built.
    /// </summary>
    /// <param name="name">The name to assign to the garage. Cannot be null or empty.</param>
    /// <returns>The current <see cref="GarageEntityBuilder"/> instance, allowing for method chaining.</returns>
    public GarageEntityBuilder AddName(string name)
    {
        this.name = name;

        return this;
    }

    /// <summary>
    /// Constructs and returns a new instance of <see cref="GarageEntity"/> using the current state of the builder.
    /// </summary>
    /// <returns>A new instance of <see cref="GarageEntity"/> populated with the builder's current state.</returns>
    public GarageEntity Builder() =>
        new(
            parkId, 
            name
        );
}
