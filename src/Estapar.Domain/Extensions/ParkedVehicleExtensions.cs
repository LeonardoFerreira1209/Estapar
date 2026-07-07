using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Extensions;

/// <summary>
/// Extension methods for creating ParkedVehicleEntity instances from webhook requests.
/// </summary>
public static class ParkedVehicleExtensions
{
    /// <summary>
    /// Converts a <see cref="WebhookRequest"/> into a <see cref="ParkedVehicleEntity"/>, registering the vehicle as parked.
    /// </summary>
    /// <param name="request">The webhook request containing the vehicle license plate.</param>
    /// <param name="entryTrafficId">The identifier of the entry traffic record that originated the parking session.</param>
    /// <param name="garageId">The identifier of the garage where the vehicle is parked.</param>
    /// <returns>A new <see cref="ParkedVehicleEntity"/> instance.</returns>
    public static ParkedVehicleEntity ToParkedVehicleEntity(
        this WebhookRequest request,
        Guid entryTrafficId,
        Guid garageId
        )
        => new(
            request.LicensePlate,
            entryTrafficId,
            garageId
        );

    /// <summary>
    /// Converts a <see cref="ParkedVehicleEntity"/> into a <see cref="ParkedVehicleResponse"/>.
    /// </summary>
    /// <param name="entity">The parked vehicle entity to convert.</param>
    /// <returns>A new <see cref="ParkedVehicleResponse"/> instance.</returns>
    public static ParkedVehicleResponse ToResponse(
        this ParkedVehicleEntity entity
        )
        => new()
        {
            Id = entity.Id,
            LicensePlate = entity.LicensePlate,
            GarageId = entity.GarageId,
            EntryTrafficId = entity.EntryTrafficId,
            Created = entity.Created
        };

    /// <summary>
    /// Converts a collection of <see cref="ParkedVehicleEntity"/> into a list of <see cref="ParkedVehicleResponse"/>.
    /// </summary>
    /// <param name="entities">The parked vehicle entities to convert.</param>
    /// <returns>A list of <see cref="ParkedVehicleResponse"/> instances.</returns>
    public static List<ParkedVehicleResponse> ToResponse(
        this IList<ParkedVehicleEntity> entities
        )
        => [.. entities.Select(e => e.ToResponse())];
}
