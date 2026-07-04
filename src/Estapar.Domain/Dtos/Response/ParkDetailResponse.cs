namespace Estapar.Domain.Dtos.Response;

/// <summary>
/// Response DTO for park detailed information including all associated lanes and garages.
/// </summary>
public class ParkDetailResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the park.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the park.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the park.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when this park was created.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this park was last updated.
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Gets or sets the collection of lanes associated with this park.
    /// </summary>
    public List<LaneResponse> Lanes { get; set; } = new();

    /// <summary>
    /// Gets or sets the collection of garages associated with this park.
    /// </summary>
    public List<GarageResponse> Garages { get; set; } = new();
}
