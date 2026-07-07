using Estapar.Domain.Builders.Create;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Extensions;

/// <summary>
/// Extension methods for converting between LaneEntity and Lane DTOs.
/// </summary>
public static class LaneExtensions
{
    /// <summary>
    /// Converts a <see cref="CreateLaneRequest"/> to a <see cref="LaneEntity"/>.
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <param name="parkId">The park identifier to associate with the lane.</param>
    /// <returns>A new <see cref="LaneEntity"/> instance.</returns>
    public static LaneEntity ToEntity(
        this CreateLaneRequest request,
        Guid parkId
    )
        => LaneCreate.CreateDefault(
            parkId,
            request.Name,
            request.LaneType,
            request.Status
        );

    /// <summary>
    /// Converts an <see cref="UpdateLaneRequest"/> to a new <see cref="LaneEntity"/>.
    /// Used when adding a new lane through an update request (no existing Id).
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <param name="parkId">The park identifier to associate with the lane.</param>
    /// <returns>A new <see cref="LaneEntity"/> instance.</returns>
    public static LaneEntity ToEntity(
        this UpdateLaneRequest request,
        Guid parkId
    )
        => LaneCreate.CreateDefault(
            parkId,
            request.Name,
            request.LaneType,
            request.Status
        );

    /// <summary>
    /// Converts a LaneEntity to a LaneResponse.
    /// </summary>
    /// <param name="entity">The entity to convert.</param>
    /// <returns>A new LaneResponse instance.</returns>
    public static LaneResponse ToResponse(this LaneEntity entity)
    {
        return new LaneResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            LaneType = entity.LaneType,
            Status = entity.Status,
            Created = entity.Created,
            Updated = entity.Updated
        };
    }

    /// <summary>
    /// Updates an existing LaneEntity from an UpdateLaneRequest.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="request">The request containing updated values.</param>
    public static LaneEntity UpdateFromRequest(
        this LaneEntity entity,
        UpdateLaneRequest request
        )
    {
        entity.Update(
            request.Name,
            request.LaneType,
            request.Status
        );

        return entity;
    }
}
