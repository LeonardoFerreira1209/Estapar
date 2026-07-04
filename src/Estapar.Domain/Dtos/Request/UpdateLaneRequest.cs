using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for updating an existing lane.
/// </summary>
public class UpdateLaneRequest
{
    /// <summary>
    /// Gets or sets the identifier of the lane (optional for new lanes).
    /// </summary>
    public Guid? Id { get; set; }

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
