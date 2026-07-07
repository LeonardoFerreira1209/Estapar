using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;

namespace SimuladorEstapar.Clients;

/// <summary>
/// Friendly facade over the Refit-generated <see cref="IEstaparApi"/> client: unwraps the
/// <c>ApiResult&lt;T&gt;</c> envelope and exposes plain domain types to the simulation services.
/// </summary>
public sealed class EstaparApiClient(IEstaparApi api)
{
    /// <summary>
    /// Retrieves every park registered in the main application (basic information only).
    /// </summary>
    public async Task<List<ParkResponse>> GetParksAsync(
        CancellationToken cancellationToken
        )
    {
        var result = 
            await api.GetParksAsync(
                cancellationToken
            );

        return result?.Data ?? [];
    }

    /// <summary>
    /// Retrieves a park by its identifier, including its entry and exit lanes.
    /// </summary>
    public async Task<ParkDetailResponse?> GetParkDetailAsync(
        Guid parkId, 
        CancellationToken cancellationToken
        )
    {
        var result = 
            await api.GetParkDetailAsync(
                parkId, 
                cancellationToken
            );

        return result?.Data;
    }

    /// <summary>
    /// Retrieves every vehicle currently parked across all garages of the given park.
    /// Used to bootstrap the simulator's in-memory state of parked vehicles on startup.
    /// </summary>
    public async Task<List<ParkedVehicleResponse>> GetParkedVehiclesAsync(
        Guid parkId,
        CancellationToken cancellationToken
        )
    {
        var result =
            await api.GetParkedVehiclesAsync(
                parkId,
                cancellationToken
            );

        return result?.Data ?? [];
    }

    /// <summary>
    /// Sends a vehicle lifecycle event (ENTRY, PARKED, or EXIT) to the webhook endpoint.
    /// The main application processes the event and, for ENTRY, broadcasts a SignalR notification.
    /// </summary>
    public Task SendWebhookAsync(
        WebhookRequest request, 
        CancellationToken cancellationToken
        )
    {
        return api.SendWebhookAsync(
            request, 
            cancellationToken
        );
    }
}
