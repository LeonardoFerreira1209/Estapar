using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Events;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Estapar.Application.Services;

/// <summary>
/// Thread-safe singleton registry that maintains one unbounded <see cref="Channel{T}"/> per lane.
/// </summary>
public sealed class LaneChannelRegistry : ILaneChannelRegistry
{
    /// <summary>
    /// Thread-safe dictionary that holds one dedicated channel per lane identifier.
    /// </summary>
    private readonly ConcurrentDictionary<Guid, Channel<LaneArrivalMessage>> _channels = new();

    /// <summary>
    /// Returns the existing channel for the given lane, or creates and registers a new unbounded
    /// channel if one does not yet exist. The channel is configured with a single reader and
    /// multiple writers, ensuring safe consumption by a single <see cref="System.Threading.Channels.ChannelReader{T}"/>.
    /// </summary>
    /// <param name="laneId">The unique identifier of the lane for which the channel should be retrieved or created.</param>
    /// <returns>The <see cref="Channel{T}"/> associated with the specified lane.</returns>
    public Channel<LaneArrivalMessage> GetOrRegister(Guid laneId)
        => _channels.GetOrAdd(laneId, _ => Channel.CreateUnbounded<LaneArrivalMessage>(
            new UnboundedChannelOptions { SingleReader = true, SingleWriter = false }));

    /// <summary>
    /// Returns a read-only dictionary containing the <see cref="ChannelReader{T}"/> for every
    /// currently registered lane. Used by the listener service to start or detect new consumers
    /// without exposing the channel writers.
    /// </summary>
    /// <returns>
    /// An <see cref="IReadOnlyDictionary{TKey,TValue}"/> mapping each lane <see cref="Guid"/>
    /// to its corresponding <see cref="ChannelReader{T}"/>.
    /// </returns>
    public IReadOnlyDictionary<Guid, ChannelReader<LaneArrivalMessage>> GetAllReaders()
        => _channels.ToDictionary(kv => kv.Key, kv => kv.Value.Reader);
}
