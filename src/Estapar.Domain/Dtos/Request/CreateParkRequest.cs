namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for creating a new park with its associated lanes and garages.
/// </summary>
public class CreateParkRequest
{
    /// <summary>
    /// Gets or sets the name of the park.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the park.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of lanes to be created with the park.
    /// </summary>
    public List<CreateLaneRequest> Lanes { get; set; } = new();

    /// <summary>
    /// Gets or sets the collection of garages to be created with the park.
    /// </summary>
    public List<CreateGarageRequest> Garages { get; set; } = new();
}
