using Estapar.Domain.Entities;

namespace Estapar.Domain.Contracts.Hubs;

/// <summary>
/// Defines the contract for broadcasting real-time vehicle arrival events
/// to SignalR listeners connected to a specific lane or park.
/// </summary>
public interface ILaneHubService
{
    /// <summary>
    /// Notifies all SignalR listeners — both park-level and lane-level —
    /// that a vehicle has arrived at the specified lane.
    /// </summary>
    /// <param name="lane">The lane entity where the vehicle arrived.</param>
    /// <param name="plate">The vehicle license plate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task NotifyVehicleArrivalAsync(
        LaneEntity lane,
        string plate,
        CancellationToken cancellationToken = default
    );
}
