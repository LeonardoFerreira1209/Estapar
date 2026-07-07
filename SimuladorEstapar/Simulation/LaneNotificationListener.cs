using Estapar.Domain.Dtos.Hub;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Enums.Lane;
using Estapar.Domain.Enums.Traffic;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using SimulatorEstapar.Clients;

namespace SimulatorEstapar.Simulation;

/// <summary>
/// Owns the SignalR connections used to react to vehicle arrival notifications broadcast by
/// the main application's <c>LaneHub</c>.
/// </summary>
/// <remarks>
/// Closes the loop Simulator -&gt; Webhook (ENTRY) -&gt; WebhookService -&gt; LaneHub -&gt; Listener:
/// opens one dedicated connection per lane (<c>?parkId=&amp;laneId=</c>) so every notification
/// received can be checked against the lane it was meant for, and, whenever the notification
/// confirms an ENTRY on one of the park's entry lanes, simulates the vehicle driving to a spot
/// and calls the webhook back with the PARKED event for that same vehicle and lane.
/// </remarks>
public sealed class LaneNotificationListener : IAsyncDisposable
{
    private readonly List<HubConnection> _connections = [];

    private LaneNotificationListener()
    {
    }

    /// <summary>
    /// Connects to the lane hub for the given park and all of its lanes.
    /// </summary>
    /// <param name="hubBaseUrl">Base URL of the main application's SignalR hub.</param>
    /// <param name="park">The park being simulated, including its lanes.</param>
    /// <param name="apiClient">Client used to call the webhook back with the PARKED event.</param>
    /// <param name="onVehicleParked">Invoked with the plate once the PARKED webhook call succeeds.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public static async Task<LaneNotificationListener> ConnectAsync(
        string hubBaseUrl,
        ParkDetailResponse park,
        EstaparApiClient apiClient,
        Action<string> onVehicleParked,
        CancellationToken cancellationToken)
    {
        var listener = new LaneNotificationListener();

        foreach (var lane in park.Lanes)
            await listener.StartConnectionAsync(
                $"{hubBaseUrl}?parkId={park.Id}&laneId={lane.Id}",
                lane,
                apiClient,
                onVehicleParked,
                cancellationToken
            );

        return listener;
    }

    /// <summary>
    /// Builds, starts, and tracks a hub connection targeting the given lane's group.
    /// </summary>
    private async Task StartConnectionAsync(
        string url,
        LaneResponse lane,
        EstaparApiClient apiClient,
        Action<string> onVehicleParked,
        CancellationToken cancellationToken)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        connection.On<VehicleArrivalNotification>(
            "OnVehicleArrival",
            notification => OnVehicleArrivalAsync(
                notification,
                lane,
                apiClient,
                onVehicleParked,
                cancellationToken
            )
        );

        await connection
            .StartAsync(
                cancellationToken
            );

        _connections.Add(connection);
    }

    /// <summary>
    /// Reacts to a vehicle arrival notification received from the hub: logs whether it arrived
    /// on the expected lane and, when it confirms an ENTRY on this entry lane, simulates the
    /// drive to a parking spot and calls the webhook back with the PARKED event for the same
    /// vehicle and lane.
    /// </summary>
    private static async Task OnVehicleArrivalAsync(
        VehicleArrivalNotification notification,
        LaneResponse lane,
        EstaparApiClient apiClient,
        Action<string> onVehicleParked,
        CancellationToken cancellationToken)
    {
        var isCorrectLane = notification.LaneId == lane.Id;

        Log.Information(
            "[HUB:LANE {LaneName}] veiculo {Plate} as {ArrivedAt:HH:mm:ss} - lane correta: {IsCorrectLane}.",
            lane.Name,
            notification.Plate,
            notification.ArrivedAt.ToLocalTime(),
            isCorrectLane
        );

        if (!isCorrectLane)
        {
            Log.Warning(
                "[HUB:LANE {LaneName}] recebeu notificacao de outra lane ({NotificationLaneId}).",
                lane.Name,
                notification.LaneId
            );

            return;
        }

        if (lane.LaneType != LaneType.Entry)
            return;

        try
        {
            await Task.Delay(
                TimeSpan.FromSeconds(Random.Shared.Next(2, 6)),
                cancellationToken
            );

            var (lat, lng) = GenerateRandomCoordinates();

            await apiClient.SendWebhookAsync(
                new WebhookRequest(
                    LicensePlate: notification.Plate,
                    LaneId: lane.Id,
                    EventType: TrafficAction.Park,
                    EntryTime: DateTime.UtcNow,
                    ExitTime: null,
                    Lat: lat,
                    Lng: lng
                ),
                cancellationToken
            );

            onVehicleParked(notification.Plate);

            Log.Information("[ESTACIONADO] veiculo {Plate} estacionou (entrada pela lane {LaneName}), apos notificacao do hub.",
                notification.Plate,
                lane.Name
            );
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Host is shutting down.
        }
        catch (Exception exception)
        {
            Log.Error(
                exception,
                "Erro ao chamar o webhook de PARKED para o veiculo {Plate} apos notificacao do hub.",
                notification.Plate
            );
        }
    }

    /// <summary>
    /// Generates plausible latitude/longitude coordinates (scattered around a fixed origin) for
    /// the simulated PARKED event.
    /// </summary>
    private static (double Lat, double Lng) GenerateRandomCoordinates()
    {
        const double originLat = -23.550520;
        const double originLng = -46.633308;

        var lat = originLat + (Random.Shared.NextDouble() - 0.5) * 0.05;
        var lng = originLng + (Random.Shared.NextDouble() - 0.5) * 0.05;

        return (lat, lng);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        foreach (var connection in _connections)
            await connection.DisposeAsync();
    }
}

