using Microsoft.AspNetCore.SignalR;

namespace Estapar.Application.Hubs;

/// <summary>
/// SignalR hub that groups connected clients by park and, optionally, by lane, so that
/// vehicle arrival notifications can be broadcast to the right audience.
/// </summary>
/// <remarks>
/// Clients must connect providing at least the <c>parkId</c> query string parameter:
/// <list type="bullet">
///   <item>
///     <description>
///       <c>/hubs/lane?parkId={parkId}</c> — joins the park-level group and receives
///       arrivals from every lane in the park.
///     </description>
///   </item>
///   <item>
///     <description>
///       <c>/hubs/lane?parkId={parkId}&amp;laneId={laneId}</c> — joins the lane-level
///       group and receives arrivals only from that specific lane.
///     </description>
///   </item>
/// </list>
/// A connection is added to exactly one of these groups, so no client ever receives the
/// same event twice.
/// </remarks>
public sealed class LaneHub : Hub
{
    /// <summary>
    /// Builds the SignalR group name used by park-level subscribers.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    public static string ParkGroup(Guid parkId)
        => $"park:{parkId}";

    /// <summary>
    /// Builds the SignalR group name used by lane-level subscribers.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park.</param>
    /// <param name="laneId">The unique identifier of the lane.</param>
    public static string LaneGroup(Guid parkId, Guid laneId)
        => $"park:{parkId}:lane:{laneId}";

    /// <summary>
    /// Adds the newly established connection to the appropriate group based on the
    /// <c>parkId</c> and <c>laneId</c> query string parameters.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var query = Context.GetHttpContext()?.Request.Query;

        if (query is not null
            && Guid.TryParse(query["parkId"], out var parkId))
        {
            var group =
                Guid.TryParse(query["laneId"], out var laneId)
                    ? LaneGroup(parkId, laneId)
                    : ParkGroup(parkId);

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                group
            );
        }

        await base.OnConnectedAsync();
    }
}
