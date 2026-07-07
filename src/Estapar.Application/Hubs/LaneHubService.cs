using Estapar.Domain.Contracts.Hubs;
using Estapar.Domain.Dtos.Hub;
using Estapar.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Estapar.Application.Hubs;

/// <summary>
/// Implementation of <see cref="ILaneHubService"/> that broadcasts vehicle arrival
/// notifications to SignalR groups.
/// </summary>
/// <remarks>
/// Sends the same <see cref="VehicleArrivalNotification"/> to two groups in parallel:
/// <list type="bullet">
///   <item><description><c>park:{parkId}</c> — catches park-level subscribers.</description></item>
///   <item><description><c>park:{parkId}:lane:{laneId}</c> — catches lane-level subscribers.</description></item>
/// </list>
/// Because a connected client belongs to exactly one of these groups (enforced by <see cref="LaneHub"/>),
/// no client receives the event twice.
/// </remarks>
public sealed class LaneHubService(
    IHubContext<LaneHub> hubContext
    ) : ILaneHubService
{
    private const string VehicleArrivalEvent = "OnVehicleArrival";

    /// <summary>
    /// Notify vehicle arrival.s
    /// </summary>
    /// <param name="lane"></param>
    /// <param name="plate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task NotifyVehicleArrivalAsync(
        LaneEntity lane,
        string plate,
        CancellationToken cancellationToken = default
        )
    {
        var notification = new VehicleArrivalNotification(
            lane.Id,
            lane.ParkId,
            lane.Name,
            lane.LaneType,
            plate.ToUpperInvariant(),
            DateTime.UtcNow
        );

        await hubContext.Clients
            .Group(LaneHub.LaneGroup(lane.ParkId, lane.Id))
            .SendAsync(
                VehicleArrivalEvent,
                notification,
                cancellationToken
            );
    }
}

