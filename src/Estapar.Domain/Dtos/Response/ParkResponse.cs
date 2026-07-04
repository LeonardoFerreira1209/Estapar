namespace Estapar.Domain.Dtos.Response;

/// <summary>
/// Response DTO for park basic information (without nested collections).
/// </summary>
public class ParkResponse
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
}
