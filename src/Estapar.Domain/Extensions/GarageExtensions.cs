using Estapar.Domain.Builders.Create;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Extensions;

/// <summary>
/// Extension methods for converting between GarageEntity and Garage DTOs.
/// </summary>
public static class GarageExtensions
{
    /// <summary>
    /// Converts a <see cref="CreateGarageRequest"/> to a <see cref="GarageEntity"/>.
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <param name="parkId">The park identifier to associate with the garage.</param>
    /// <returns>A new <see cref="GarageEntity"/> instance.</returns>
    public static GarageEntity ToEntity(
        this CreateGarageRequest request,
        Guid parkId
    )
        => GarageCreate.CreateDefault(
            parkId, 
            request.Name
        );

    /// <summary>
    /// Converts an <see cref="UpdateGarageRequest"/> to a new <see cref="GarageEntity"/>.
    /// Used when adding a new garage through an update request (no existing Id).
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <param name="parkId">The park identifier to associate with the garage.</param>
    /// <returns>A new <see cref="GarageEntity"/> instance.</returns>
    public static GarageEntity ToEntity(
        this UpdateGarageRequest request,
        Guid parkId
    )
        => GarageCreate.CreateDefault(
            parkId, 
            request.Name
        );

    /// <summary>
    /// Converts a GarageEntity to a GarageResponse.
    /// </summary>
    /// <param name="entity">The entity to convert.</param>
    /// <returns>A new GarageResponse instance.</returns>
    public static GarageResponse ToResponse(this GarageEntity entity)
    {
        return new GarageResponse
        {
            Id = entity.Id,
            ParkId = entity.ParkId,
            Name = entity.Name,
            Created = entity.Created,
            Updated = entity.Updated
        };
    }

    /// <summary>
    /// Updates an existing GarageEntity from an UpdateGarageRequest.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="request">The request containing updated values.</param>
    public static void UpdateFromRequest(
        this GarageEntity entity,
        UpdateGarageRequest request
        )
    {
        entity.Update(request.Name);
    }
}
