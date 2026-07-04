namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for updating an existing park with its associated lanes and garages.
/// </summary>
public class UpdateParkRequest
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
    /// Gets or sets the collection of lanes. Existing lanes should include their ID, new lanes should not.
    /// </summary>
    public List<UpdateLaneRequest> Lanes { get; set; } = new();

    /// <summary>
    /// Gets or sets the collection of garages. Existing garages should include their ID, new garages should not.
    /// </summary>
    public List<UpdateGarageRequest> Garages { get; set; } = new();
}
