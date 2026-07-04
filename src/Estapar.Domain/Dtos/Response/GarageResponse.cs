namespace Estapar.Domain.Dtos.Response;

/// <summary>
/// Response DTO for garage information.
/// </summary>
public class GarageResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the garage.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the park to which this garage belongs.
    /// </summary>
    public Guid ParkId { get; set; }

    /// <summary>
    /// Gets or sets the name of the garage.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when this garage was created.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this garage was last updated.
    /// </summary>
    public DateTime? Updated { get; set; }
}
