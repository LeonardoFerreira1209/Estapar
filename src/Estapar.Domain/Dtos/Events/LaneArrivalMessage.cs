using Estapar.Domain.Entities;

namespace Estapar.Domain.Dtos.Events;

/// <summary>
/// Represents the internal event that flows through a lane's dedicated channel.
/// </summary>
/// <param name="Lane">The lane entity where the vehicle arrived.</param>
/// <param name="Plate">The vehicle license plate.</param>
public record LaneArrivalMessage(
    LaneEntity Lane, 
    string Plate
);
