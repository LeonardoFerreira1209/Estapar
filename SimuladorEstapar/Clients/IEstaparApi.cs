using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Dtos.Results;
using Refit;

namespace SimuladorEstapar.Clients;

/// <summary>
/// Refit definition of the main Estapar application's HTTP surface consumed by the simulator:
/// park/lane discovery and vehicle lifecycle events (ENTRY, PARKED, EXIT) posted to
/// <c>WebhookController</c>.
/// </summary>
public interface IEstaparApi
{
    /// <summary>
    /// Retrieves every park registered in the main application (basic information only).
    /// </summary>
    [Get("/api/estapar/v1/parks")]
    Task<ApiResult<List<ParkResponse>>> GetParksAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves a park by its identifier, including its entry and exit lanes.
    /// </summary>
    [Get("/api/estapar/v1/parks/{parkId}")]
    Task<ApiResult<ParkDetailResponse>> GetParkDetailAsync(
        Guid parkId, 
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves every vehicle currently parked across all garages of the given park.
    /// Used to bootstrap the simulator's in-memory state of parked vehicles on startup.
    /// </summary>
    [Get("/api/estapar/v1/parks/{parkId}/parked-vehicles")]
    Task<ApiResult<List<ParkedVehicleResponse>>> GetParkedVehiclesAsync(
        Guid parkId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sends a vehicle lifecycle event (ENTRY, PARKED, or EXIT) to the webhook endpoint.
    /// The main application processes the event and, for ENTRY, broadcasts a SignalR notification.
    /// </summary>
    [Post("/webhook")]
    Task SendWebhookAsync(
        [Body] WebhookRequest request,
        CancellationToken cancellationToken = default
    );
}