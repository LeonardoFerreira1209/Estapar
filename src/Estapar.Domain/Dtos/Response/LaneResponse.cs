using Estapar.Domain.Enums;

using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Dtos.Response;

/// <summary>
/// Response DTO for lane information.
/// </summary>
public class LaneResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the lane.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the lane.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the lane.
    /// </summary>
    public LaneType LaneType { get; set; }

    /// <summary>
    /// Gets or sets the operational status of the lane.
    /// </summary>
    public LaneStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this lane was created.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this lane was last updated.
    /// </summary>
    public DateTime? Updated { get; set; }
}
