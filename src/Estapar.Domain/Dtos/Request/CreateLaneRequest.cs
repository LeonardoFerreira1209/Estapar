using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for creating a new lane.
/// </summary>
public class CreateLaneRequest
{
    /// <summary>
    /// Gets or sets the name of the lane.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the lane (Entry or Exit).
    /// </summary>
    public LaneType LaneType { get; set; }

    /// <summary>
    /// Gets or sets the operational status of the lane.
    /// </summary>
    public LaneStatus Status { get; set; }
}
