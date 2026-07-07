namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for updating the base data of an existing park.
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
}
