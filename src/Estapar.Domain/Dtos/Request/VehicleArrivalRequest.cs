namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request payload for simulating a vehicle arrival at a lane.
/// The park and lane are provided via the route; only the plate is required in the body.
/// </summary>
/// <param name="Plate">The vehicle license plate.</param>
public record VehicleArrivalRequest(string Plate);
