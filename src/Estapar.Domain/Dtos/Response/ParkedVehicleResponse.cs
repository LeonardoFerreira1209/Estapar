namespace Estapar.Domain.Dtos.Response;

/// <summary>
/// Response DTO for a vehicle currently parked in a garage.
/// </summary>
public class ParkedVehicleResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the parked vehicle record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the vehicle license plate.
    /// </summary>
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the garage where the vehicle is currently parked.
    /// </summary>
    public Guid GarageId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the entry traffic record that originated this parking session.
    /// </summary>
    public Guid EntryTrafficId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the vehicle entered (i.e., when this record was created).
    /// </summary>
    public DateTime Created { get; set; }
}
