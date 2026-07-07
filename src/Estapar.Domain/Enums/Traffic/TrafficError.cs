using System.ComponentModel;

namespace Estapar.Domain.Enums.Traffic;

/// <summary>
/// Defines the possible error types that can occur during a traffic attempt at a parking lane.
/// </summary>
public enum TrafficError
{
    /// <summary>
    /// No error occurred.
    /// </summary>
    [Description("No error")]
    None = 0,

    /// <summary>
    /// The vehicle is attempting to exit but is not registered as inside the parking facility.
    /// </summary>
    [Description("Vehicle not found inside the parking facility")]
    VehicleNotInside = 1,

    /// <summary>
    /// The vehicle is attempting to enter but is already registered as inside the parking facility.
    /// </summary>
    [Description("Vehicle already inside the parking facility")]
    VehicleAlreadyInside = 2,

    /// <summary>
    /// The lane is currently inactive or unavailable for use.
    /// </summary>
    [Description("Lane is inactive or unavailable")]
    LaneUnavailable = 3,

    /// <summary>
    /// An unexpected system error occurred during the traffic attempt.
    /// </summary>
    [Description("Unexpected system error")]
    SystemError = 4,

    /// <summary>
    /// No available parking spots in any garage.
    /// </summary>
    [Description("No available parking spots in any garage")]
    GarageFull = 5
}
