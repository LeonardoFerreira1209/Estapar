using Estapar.Domain.Enums.Traffic;
using System.Text.Json.Serialization;

namespace Estapar.Domain.Dtos.Request;

/// <summary>
/// Request payload received by the webhook endpoint for vehicle entry, parked, and exit events.
/// </summary>
/// <param name="LicensePlate">The vehicle license plate.</param>
/// <param name="EventType">The type of event: ENTRY, PARKED, or EXIT.</param>
/// <param name="EntryTime">The date and time the vehicle entered. Only present for ENTRY events.</param>
/// <param name="ExitTime">The date and time the vehicle exited. Only present for EXIT events.</param>
/// <param name="Lat">The latitude of the parked vehicle. Only present for PARKED events.</param>
/// <param name="Lng">The longitude of the parked vehicle. Only present for PARKED events.</param>
public record WebhookRequest(
    [property: JsonPropertyName("license_plate")] string LicensePlate,
    [property: JsonPropertyName("lane_id")] Guid LaneId,
    [property: JsonPropertyName("event_type")] TrafficAction EventType,
    [property: JsonPropertyName("entry_time")] DateTime? EntryTime,
    [property: JsonPropertyName("exit_time")] DateTime? ExitTime,
    [property: JsonPropertyName("lat")] double? Lat,
    [property: JsonPropertyName("lng")] double? Lng
);
