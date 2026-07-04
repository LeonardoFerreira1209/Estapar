using Estapar.Domain.Builders;
using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Builders.Create;

/// <summary>
/// Creates a default lane entity with the specified initial data.
/// </summary>
public static class LaneCreate
{
    /// <summary>
    /// Creates a default <see cref="LaneEntity"/> instance with the specified attributes.
    /// </summary>
    /// <param name="parkId">The unique identifier of the parent park. Cannot be empty.</param>
    /// <param name="name">The name of the lane. Cannot be null or empty.</param>
    /// <param name="laneType">The type of the lane (Entry or Exit).</param>
    /// <param name="status">The operational status of the lane.</param>
    /// <returns>A new <see cref="LaneEntity"/> instance initialized with the provided attributes.</returns>
    public static LaneEntity CreateDefault(
        Guid parkId,
        string name,
        LaneType laneType,
        LaneStatus status
    )
        => new LaneEntityBuilder()
            .AddParkId(parkId)
            .AddName(name)
            .AddLaneType(laneType)
            .AddStatus(status)
            .Builder();
}
