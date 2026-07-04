using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Builders;

/// <summary>
/// Provides a fluent interface for building and configuring a lane entity.
/// </summary>
public sealed class LaneEntityBuilder
{
    private Guid parkId;
    private string name;
    private LaneType laneType;
    private LaneStatus status;

    /// <summary>
    /// Sets the park identifier for the lane being built.
    /// </summary>
    /// <param name="parkId">The unique identifier of the parent park.</param>
    /// <returns>The current <see cref="LaneEntityBuilder"/> instance, allowing for method chaining.</returns>
    public LaneEntityBuilder AddParkId(Guid parkId)
    {
        this.parkId = parkId;

        return this;
    }

    /// <summary>
    /// Sets the name for the lane being built.
    /// </summary>
    /// <param name="name">The name to assign to the lane. Cannot be null or empty.</param>
    /// <returns>The current <see cref="LaneEntityBuilder"/> instance, allowing for method chaining.</returns>
    public LaneEntityBuilder AddName(string name)
    {
        this.name = name;

        return this;
    }

    /// <summary>
    /// Sets the lane type for the lane being built.
    /// </summary>
    /// <param name="laneType">The type of the lane (Entry or Exit).</param>
    /// <returns>The current <see cref="LaneEntityBuilder"/> instance, allowing for method chaining.</returns>
    public LaneEntityBuilder AddLaneType(LaneType laneType)
    {
        this.laneType = laneType;

        return this;
    }

    /// <summary>
    /// Sets the operational status for the lane being built.
    /// </summary>
    /// <param name="status">The operational status of the lane.</param>
    /// <returns>The current <see cref="LaneEntityBuilder"/> instance, allowing for method chaining.</returns>
    public LaneEntityBuilder AddStatus(LaneStatus status)
    {
        this.status = status;

        return this;
    }

    /// <summary>
    /// Constructs and returns a new instance of <see cref="LaneEntity"/> using the current state of the builder.
    /// </summary>
    /// <returns>A new instance of <see cref="LaneEntity"/> populated with the builder's current state.</returns>
    public LaneEntity Builder() =>
        new(
            parkId,
            name,
            laneType,
            status
        );
}
