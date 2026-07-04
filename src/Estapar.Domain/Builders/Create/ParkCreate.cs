using Estapar.Domain.Entities;

namespace Estapar.Domain.Builders.Create;

/// <summary>
/// Creates a default park entity with the specified initial data.
/// </summary>
public static class ParkCreate
{
    /// <summary>
    /// Creates a default <see cref="ParkEntity"/> instance with the specified attributes.
    /// </summary>
    /// <param name="id">The unique identifier for the park.</param>
    /// <param name="name">The name of the park. Cannot be null or empty.</param>
    /// <param name="description">A description of the park. Can be null or empty.</param>
    /// <param name="lanes">The initial list of lanes associated with the park. Cannot be null.</param>
    /// <param name="garages">The initial list of garages associated with the park. Cannot be null.</param>
    /// <returns>A new <see cref="ParkEntity"/> instance initialized with the provided attributes.</returns>
    public static ParkEntity CreateDefault(
        Guid id,
        string name,
        string description,
        List<LaneEntity> lanes,
        List<GarageEntity> garages
    )
        => new ParkEntityBuilder()
            .AddId(id)
            .AddName(name)
            .AddDescription(description)
            .AddLanes(lanes)
            .AddGarages(garages)
            .Builder();
}
