namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request DTO for updating an existing garage.
/// </summary>
public class UpdateGarageRequest
{
    /// <summary>
    /// Gets or sets the identifier of the garage (optional for new garages).
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the garage.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
