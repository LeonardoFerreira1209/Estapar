using Estapar.Domain.Enums.Lane;

namespace Estapar.Domain.Dtos.Hub;

/// <summary>
/// Payload transmitted via SignalR to all listeners when a vehicle arrives at a lane.
/// </summary>
/// <param name="LaneId">The unique identifier of the lane where the vehicle arrived.</param>
/// <param name="ParkId">The unique identifier of the park that owns the lane.</param>
/// <param name="LaneName">The descriptive name of the lane.</param>
/// <param name="LaneType">Indicates whether this is an entry or exit lane.</param>
/// <param name="Plate">The vehicle license plate (normalized to upper-case).</param>
/// <param name="ArrivedAt">UTC timestamp when the vehicle arrival was registered.</param>
public record VehicleArrivalNotification(
    Guid LaneId,
    Guid ParkId,
    string LaneName,
    LaneType LaneType,
    string Plate,
    DateTime ArrivedAt
);
