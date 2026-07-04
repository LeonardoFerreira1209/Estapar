using Estapar.Domain.Entities;

namespace Estapar.Domain.Builders.Create;

/// <summary>
/// Creates a default garage entity with the specified initial data.
/// </summary>
public static class GarageCreate
{
    /// <summary>
    /// Creates a default <see cref="GarageEntity"/> instance with the specified attributes.
    /// </summary>
    /// <param name="parkId">The unique identifier of the parent park. Cannot be empty.</param>
    /// <param name="name">The name of the garage. Cannot be null or empty.</param>
    /// <returns>A new <see cref="GarageEntity"/> instance initialized with the provided attributes.</returns>
    public static GarageEntity CreateDefault(
            Guid parkId, 
            string name
    )
        => new GarageEntityBuilder()
            .AddParkId(parkId)
            .AddName(name)
            .Builder();
}
