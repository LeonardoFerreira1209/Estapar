using Estapar.Domain.Builders.Create;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Extensions;

/// <summary>
/// Extension methods for converting between ParkEntity and Park DTOs.
/// </summary>
public static class ParkExtensions
{
    /// <summary>
    /// Converts a CreateParkRequest to a ParkEntity with associated lanes and garages.
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <returns>A new ParkEntity instance with its lanes and garages.</returns>
    public static ParkEntity ToEntity(this CreateParkRequest request)
    {
        var parkId = Guid.NewGuid();

        var lanes = request.Lanes
            .Select(l => LaneCreate.CreateDefault(
                    parkId, 
                    l.Name, 
                    l.LaneType, 
                    l.Status
                )
            )
            .ToList();

        var garages = request.Garages
            .Select(g => GarageCreate.CreateDefault(parkId, g.Name))
            .ToList();

        return ParkCreate.CreateDefault(
            parkId,
            request.Name,
            request.Description,
            lanes,
            garages
        );
    }

    /// <summary>
    /// Converts a ParkEntity to a ParkResponse (basic information without nested collections).
    /// </summary>
    /// <param name="entity">The entity to convert.</param>
    /// <returns>A new ParkResponse instance.</returns>
    public static ParkResponse ToResponse(this ParkEntity entity)
    {
        return new ParkResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Created = entity.Created,
            Updated = entity.Updated
        };
    }

    /// <summary>
    /// Converts a ParkEntity to a ParkDetailResponse including all nested lanes and garages.
    /// </summary>
    /// <param name="entity">The entity to convert.</param>
    /// <returns>A new ParkDetailResponse instance with nested collections.</returns>
    public static ParkDetailResponse ToDetailResponse(this ParkEntity entity)
    {
        return new ParkDetailResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Created = entity.Created,
            Updated = entity.Updated,
            Lanes = entity.Lanes?.Select(l => l.ToResponse()).ToList() ?? [],
            Garages = entity.Garages?.Select(g => g.ToResponse()).ToList() ?? []
        };
    }

    /// <summary>
    /// Updates an existing ParkEntity from an UpdateParkRequest.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="request">The request containing updated values.</param>
    public static void UpdateFromRequest(
        this ParkEntity entity,
        UpdateParkRequest request)
    {
        entity.Update(
            request.Name,
            request.Description
        );
    }
}
