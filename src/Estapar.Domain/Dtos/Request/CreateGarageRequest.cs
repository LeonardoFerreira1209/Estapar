namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for creating a new garage.
/// </summary>
public class CreateGarageRequest
{
    /// <summary>
    /// Gets or sets the name of the garage.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
