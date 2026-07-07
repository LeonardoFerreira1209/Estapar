using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Enums.Lane;
using Estapar.Domain.Enums.Traffic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using SimuladorEstapar.Clients;
using SimuladorEstapar.Configuration;
using System.Collections.Concurrent;

namespace SimuladorEstapar.Simulation;

/// <summary>
/// Background service that drives the whole simulation: discovers every park registered in the
/// main application (or a single configured park via <see cref="SimulatorOptions.ParkId"/>) and
/// runs one independent simulation loop per park, in parallel. For each park, it seeds its
/// initial parked-vehicle state from the vehicles already parked in the main application, opens
/// a <see cref="LaneNotificationListener"/> per lane, and continuously simulates vehicles
/// entering and exiting via the webhook endpoint. Only the ENTRY and EXIT events are initiated
/// here; the PARKED event is sent by <see cref="LaneNotificationListener"/> once the main
/// application's hub confirms the vehicle's arrival on its entry lane, mirroring the real
/// webhook -&gt; hub notification flow instead of simulating PARK independently.
/// </summary>
public sealed class VehicleSimulationService(
    EstaparApiClient apiClient,
    IOptions<SimulatorOptions> options
) : BackgroundService
{
    private readonly SimulatorOptions _options = options.Value;

    /// <summary>
    /// Execute de vehicle park simulator.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information(
            "Simulador Estapar iniciando. API alvo: {ApiBaseUrl} | Hub: {HubBaseUrl}",
            _options.ApiBaseUrl,
            _options.ResolvedHubBaseUrl
        );

        var parks = 
            await WaitForParksAsync(
                stoppingToken
            );

        if (parks.Count == 0)
            return;

        Log.Information(
            "Simulador Estapar: {ParkCount} park(s) encontrado(s). Iniciando simulacao em paralelo.",
            parks.Count
        );

        await Task.WhenAll(
            parks.Select(
                park => RunParkSimulationAsync(
                    park,
                    stoppingToken
                )
            )
        );

        Log.Information("Simulador encerrado.");
    }

    /// <summary>
    /// Runs the full simulation loop for a single park: seeds the initial parked-vehicle state
    /// from the vehicles already parked in the main application, opens the lane listener, and
    /// keeps simulating ENTRY/EXIT events until cancellation is requested. Multiple parks run
    /// this method concurrently, each with its own isolated parked-vehicle state.
    /// </summary>
    private async Task RunParkSimulationAsync(
        ParkDetailResponse park,
        CancellationToken stoppingToken
        )
    {
        var entryLanes =
            park.Lanes.Where(
                lane => lane.LaneType == LaneType.Entry
            ).ToList();

        var exitLanes =
            park.Lanes.Where(
                lane => lane.LaneType == LaneType.Exit
            ).ToList();

        if (entryLanes.Count == 0 || exitLanes.Count == 0)
        {
            Log.Information(
                "O park {ParkName} precisa de ao menos uma lane de entrada e uma de saida para simular trafego.",
                park.Name
            );

            return;
        }

        var vehiclesParked = 
            new ConcurrentDictionary<string, byte>(
                StringComparer.OrdinalIgnoreCase
            );

        await SeedParkedVehiclesAsync(
            park,
            vehiclesParked,
            stoppingToken
        );

        await using var listener =
            await LaneNotificationListener.ConnectAsync(
                _options.ResolvedHubBaseUrl,
                park,
                apiClient,
                plate => vehiclesParked[plate] = 0,
                stoppingToken
            );

        Log.Information(
            "Simulador iniciado para o park {ParkName} ({ParkId}) - {EntryCount} lane(s) de entrada, {ExitCount} lane(s) de saida, {ParkedCount} veiculo(s) ja estacionado(s).",
            park.Name,
            park.Id,
            entryLanes.Count,
            exitLanes.Count,
            vehiclesParked.Count
        );

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SimulateNextEventAsync(
                    vehiclesParked,
                    entryLanes,
                    exitLanes,
                    stoppingToken
                );
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                Log.Error(
                    exception,
                    "Erro ao simular evento de veiculo no park {ParkName}.",
                    park.Name
                );
            }

            await DelayAsync(
                stoppingToken
            );
        }

        Log.Information(
            "Simulador do park {ParkName} encerrado.",
            park.Name
        );
    }

    /// <summary>
    /// Loads every vehicle currently parked in the given park from the main application and uses
    /// it to seed the in-memory <paramref name="vehiclesParked"/> state, so the simulator starts
    /// in sync with the real occupancy instead of assuming an empty park.
    /// </summary>
    private async Task SeedParkedVehiclesAsync(
        ParkDetailResponse park,
        ConcurrentDictionary<string, byte> vehiclesParked,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var parkedVehicles =
                await apiClient.GetParkedVehiclesAsync(
                    park.Id,
                    cancellationToken
                );

            foreach (var vehicle in parkedVehicles)
                vehiclesParked[vehicle.LicensePlate] = 0;

            Log.Information(
                "Park {ParkName} ({ParkId}): {Count} veiculo(s) ja estacionado(s) carregado(s) para o estado inicial do simulador.",
                park.Name,
                park.Id,
                parkedVehicles.Count
            );
        }
        catch (Exception exception)
        {
            Log.Error(
                exception,
                "Erro ao carregar veiculos ja estacionados no park {ParkName} ({ParkId}). Iniciando com estado vazio.",
                park.Name,
                park.Id
            );
        }
    }

    /// <summary>
    /// Simulates either a vehicle exit (favored once enough vehicles are parked, so the
    /// simulated occupancy does not grow indefinitely) or a new vehicle entry.
    /// </summary>
    private async Task SimulateNextEventAsync(
        ConcurrentDictionary<string, byte> vehiclesParked,
        List<LaneResponse> entryLanes,
        List<LaneResponse> exitLanes,
        CancellationToken cancellationToken
        )
    {
        var shouldSimulateExit =
            !(vehiclesParked.IsEmpty
            || vehiclesParked.Count <= 10
                && Random.Shared.Next(100) >= 50
            );

        if (shouldSimulateExit && TryTakeParkedVehicle(
            vehiclesParked, 
            out var exitingPlate
            )
        )
        {
            await SimulateExitAsync(
                exitingPlate,
                exitLanes,
                cancellationToken
            );

            return;
        }

        await SimulateEntryAsync(
            entryLanes,
            cancellationToken
        );
    }

    /// <summary>
    /// Sends the ENTRY webhook event for a newly generated plate through a random entry lane.
    /// The follow-up PARKED event is not scheduled here: it is sent by
    /// <see cref="LaneNotificationListener"/> once the hub confirms this vehicle's arrival on
    /// the entry lane, keeping the simulator's PARK trigger driven by the same webhook -&gt; hub
    /// notification loop the real application uses.
    /// </summary>
    private async Task SimulateEntryAsync(
        List<LaneResponse> entryLanes,
        CancellationToken cancellationToken
        )
    {
        var plate = 
            PlateGenerator.Generate();

        var entryLane =
            entryLanes[
                Random.Shared.Next(
                    entryLanes.Count
                )
            ];

        await apiClient.SendWebhookAsync(
            new WebhookRequest(
                LicensePlate: plate,
                LaneId: entryLane.Id,
                EventType: TrafficAction.Entry,
                EntryTime: DateTime.UtcNow,
                ExitTime: null,
                Lat: null,
                Lng: null
            ),
            cancellationToken
        );

        Log.Information("[ENTRADA] veiculo {Plate} entrando pela lane {LaneName}.",
            plate,
            entryLane.Name
        );
    }

    /// <summary>
    /// Sends the EXIT webhook event for an already parked vehicle through a random exit lane.
    /// </summary>
    private async Task SimulateExitAsync(
        string plate,
        List<LaneResponse> exitLanes,
        CancellationToken cancellationToken
        )
    {
        var exitLane = exitLanes[
            Random.Shared.Next(
                exitLanes.Count
            )
        ];

        await apiClient.SendWebhookAsync(
            new WebhookRequest(
                LicensePlate: plate,
                LaneId: exitLane.Id,
                EventType: TrafficAction.Exit,
                EntryTime: null,
                ExitTime: DateTime.UtcNow,
                Lat: null,
                Lng: null
            ),
            cancellationToken
        );

        Log.Information("[SAIDA] veiculo {Plate} saindo pela lane {LaneName}.",
            plate,
            exitLane.Name
        );
    }

    /// <summary>
    /// Picks and removes a random plate from the set of vehicles currently parked and eligible
    /// for an EXIT event.
    /// </summary>
    private static bool TryTakeParkedVehicle(
        ConcurrentDictionary<string, byte> vehiclesParked,
        out string plate
        )
    {
        var keys = 
            vehiclesParked
            .Keys
            .ToArray();

        if (keys.Length == 0)
        {
            plate = string.Empty;
            return false;
        }

        plate = keys[Random.Shared.Next(keys.Length)];

        return vehiclesParked.TryRemove(plate, out _);
    }

    /// <summary>
    /// Waits until the main application is reachable and has at least one park registered,
    /// retrying periodically so the simulator can be started before or after the main app.
    /// </summary>
    private async Task<List<ParkDetailResponse>> WaitForParksAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var parks = 
                    await ResolveParksAsync(
                        cancellationToken
                    );

                if (parks.Count > 0)
                    return parks;

                Log.Warning(
                    "Nenhum park cadastrado ainda em {ApiBaseUrl}. Cadastre um park com lanes de entrada e saida.",
                    _options.ApiBaseUrl
                );
            }
            catch (HttpRequestException exception)
            {
                Log.Error(
                    "Nao foi possivel conectar a Estapar.Api em {ApiBaseUrl} ({Message}). Nova tentativa em 5s.",
                    _options.ApiBaseUrl,
                    exception.Message
                );
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(5), 
                    cancellationToken
                );
            }
            catch (OperationCanceledException)
            {
                return [];
            }
        }

        return [];
    }

    /// <summary>
    /// Resolves the parks to simulate: only the configured <see cref="SimulatorOptions.ParkId"/>
    /// when present, otherwise every park currently registered in the main application, so the
    /// simulator can run all of them concurrently.
    /// </summary>
    private async Task<List<ParkDetailResponse>> ResolveParksAsync(
        CancellationToken cancellationToken
        )
    {
        if (_options.ParkId is { } configuredParkId)
        {
            var configuredPark =
                await apiClient.GetParkDetailAsync(
                    configuredParkId,
                    cancellationToken
                );

            return configuredPark is null ? [] : [configuredPark];
        }

        var parks =
            await apiClient.GetParksAsync(
                cancellationToken
            );

        var details = 
            new List<ParkDetailResponse>();

        foreach (var park in parks)
        {
            var detail =
                await apiClient.GetParkDetailAsync(
                    park.Id,
                    cancellationToken
                );

            if (detail is not null)
                details.Add(detail);
        }

        return details;
    }

    /// <summary>
    /// Waits a random interval, bounded by <see cref="SimulatorOptions.MinIntervalSeconds"/> and
    /// <see cref="SimulatorOptions.MaxIntervalSeconds"/>, between simulated events.
    /// </summary>
    private async Task DelayAsync(CancellationToken cancellationToken)
    {
        var min = 
            Math.Max(
                1, 
                _options.MinIntervalSeconds
            );

        var max = 
            Math.Max(
                min, 
                _options.MaxIntervalSeconds
            );

        var seconds = 
            Random.Shared.Next(
                min, 
                max + 1
            );

        try
        {
            await Task.Delay(
                TimeSpan.FromSeconds(seconds), 
                cancellationToken
            );
        }
        catch (OperationCanceledException)
        {
            
        }
    }
}

