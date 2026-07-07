using System.ComponentModel;

namespace Estapar.Domain.Enums.Traffic;

/// <summary>
/// Defines the action type of a traffic attempt at a parking lane.
/// </summary>
public enum TrafficAction
{
    /// <summary>
    /// Entry action - the vehicle is attempting to enter the parking facility.
    /// </summary>
    [Description("Vehicle entry attempt")]
    Entry = 1,

    /// <summary>
    /// Exit action - the vehicle is attempting to exit the parking facility.
    /// </summary>
    [Description("Vehicle exit attempt")]
    Exit = 2,

    /// <summary>
    /// Park action - the vehicle has been authorized to proceed into the parking facility.
    /// </summary>
    [Description("Vehicle park authorization")]
    Park = 3
}
