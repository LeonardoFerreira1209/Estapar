using System.ComponentModel;

namespace Estapar.Domain.Enums.Lane;

/// <summary>
/// Defines the operational status of entities in the system.
/// </summary>
public enum LaneStatus
{
    /// <summary>
    /// Active status - the entity is currently operational and available.
    /// </summary>
    [Description("Active and operational")]
    Active = 1,

    /// <summary>
    /// Inactive status - the entity is currently non-operational or unavailable.
    /// </summary>
    [Description("Inactive and unavailable")]
    Inactive = 2
}
