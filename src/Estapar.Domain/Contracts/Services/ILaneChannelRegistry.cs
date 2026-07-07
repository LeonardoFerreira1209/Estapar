using Estapar.Domain.Dtos.Events;
using System.Threading.Channels;

namespace Estapar.Domain.Contracts.Services;

/// <summary>
/// Registry that maintains a dedicated <see cref="Channel{T}"/> for each lane.
/// </summary>
/// <remarks>
/// The registry is a singleton. Lanes discovered at startup are pre-registered via
/// <see cref="GetOrRegister"/>. Lanes added later are registered lazily on the first
/// write attempt from the controller.
/// </remarks>
public interface ILaneChannelRegistry
{
    /// <summary>
    /// Returns the channel for the given lane, creating one if it does not exist yet.
    /// </summary>
    /// <param name="laneId">The lane unique identifier.</param>
    /// <returns>The <see cref="Channel{T}"/> associated with the lane.</returns>
    Channel<LaneArrivalMessage> GetOrRegister(Guid laneId);

    /// <summary>
    /// Returns a snapshot of all registered readers, keyed by lane identifier.
    /// </summary>
    IReadOnlyDictionary<Guid, ChannelReader<LaneArrivalMessage>> GetAllReaders();
}
