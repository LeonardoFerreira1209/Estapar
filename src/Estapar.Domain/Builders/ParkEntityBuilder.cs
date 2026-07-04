using Estapar.Domain.Entities;

namespace Estapar.Domain.Builders;

/// <summary>
/// Provides a fluent interface for building and configuring a park entity.
/// </summary>
public sealed class ParkEntityBuilder
{
    private Guid id = Guid.NewGuid();
    private string name;
    private string description;
    private List<LaneEntity> lanes = [];
    private List<GarageEntity> garages = [];

    /// <summary>
    /// Gets the pre-generated unique identifier for the park being built.
    /// Use this value when constructing child entities (lanes, garages) that require the parent park ID.
    /// </summary>
    public Guid Id => id;

    /// <summary>
    /// Sets the unique identifier for the park being built.
    /// </summary>
    /// <param name="id">The unique identifier to assign to the park.</param>
    /// <returns>The current <see cref="ParkEntityBuilder"/> instance, allowing for method chaining.</returns>
    public ParkEntityBuilder AddId(Guid id)
    {
        this.id = id;

        return this;
    }

    /// <summary>
    /// Sets the name for the park being built.
    /// </summary>
    /// <param name="name">The name to assign to the park. Cannot be null or empty.</param>
    /// <returns>The current <see cref="ParkEntityBuilder"/> instance, allowing for method chaining.</returns>
    public ParkEntityBuilder AddName(string name)
    {
        this.name = name;

        return this;
    }

    /// <summary>
    /// Sets the description for the park being built.
    /// </summary>
    /// <param name="description">The description to assign to the park. Can be null or empty.</param>
    /// <returns>The current <see cref="ParkEntityBuilder"/> instance, allowing for method chaining.</returns>
    public ParkEntityBuilder AddDescription(string description)
    {
        this.description = description;

        return this;
    }

    /// <summary>
    /// Sets the collection of lanes to associate with the park being built.
    /// </summary>
    /// <param name="lanes">The list of <see cref="LaneEntity"/> instances to assign. Cannot be null.</param>
    /// <returns>The current <see cref="ParkEntityBuilder"/> instance, allowing for method chaining.</returns>
    public ParkEntityBuilder AddLanes(List<LaneEntity> lanes)
    {
        this.lanes = lanes;

        return this;
    }

    /// <summary>
    /// Sets the collection of garages to associate with the park being built.
    /// </summary>
    /// <param name="garages">The list of <see cref="GarageEntity"/> instances to assign. Cannot be null.</param>
    /// <returns>The current <see cref="ParkEntityBuilder"/> instance, allowing for method chaining.</returns>
    public ParkEntityBuilder AddGarages(List<GarageEntity> garages)
    {
        this.garages = garages;

        return this;
    }

    /// <summary>
    /// Constructs and returns a new instance of <see cref="ParkEntity"/> using the current state of the builder.
    /// </summary>
    /// <returns>A new instance of <see cref="ParkEntity"/> populated with the builder's current state.</returns>
    public ParkEntity Builder() =>
        new(
            id,
            name,
            description,
            lanes,
            garages
        );
}
