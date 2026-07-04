using System.ComponentModel;

namespace Estapar.Domain.Enums.Lane;

/// <summary>
/// Defines the types of lanes in a parking facility, indicating their operational direction and purpose.
/// </summary>
public enum LaneType
{
    /// <summary>
    /// Entry lane - designated for vehicles entering the parking facility.
    /// </summary>
    [Description("Entry lane for incoming vehicles")]
    Entry = 1,

    /// <summary>
    /// Exit lane - designated for vehicles leaving the parking facility.
    /// </summary>
    [Description("Exit lane for outgoing vehicles")]
    Exit = 2
}
