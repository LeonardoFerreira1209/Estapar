namespace SimuladorEstapar.Configuration;

/// <summary>
/// Configuration options that control how the simulator connects to the main Estapar
/// application and how frequently it generates simulated vehicle traffic.
/// </summary>
public class SimulatorOptions
{
    /// <summary>
    /// Base URL of the Estapar.Api HTTP endpoints, e.g. <c>http://localhost:5292</c>.
    /// </summary>
    public string ApiBaseUrl { get; set; } = "http://localhost:5292";

    /// <summary>
    /// Base URL used to open the SignalR hub connection. Falls back to <see cref="ApiBaseUrl"/>
    /// when not explicitly set.
    /// </summary>
    public string? HubBaseUrl { get; set; }

    /// <summary>
    /// Identifier of the park to simulate. When not set, the simulator discovers every park
    /// registered in the main application and runs one simulation loop per park, in parallel.
    /// </summary>
    public Guid? ParkId { get; set; }

    /// <summary>
    /// Minimum delay, in seconds, between two simulated vehicle events.
    /// </summary>
    public int MinIntervalSeconds { get; set; } = 2;

    /// <summary>
    /// Maximum delay, in seconds, between two simulated vehicle events.
    /// </summary>
    public int MaxIntervalSeconds { get; set; } = 6;

    /// <summary>
    /// Resolves the base URL used for SignalR hub connections.
    /// </summary>
    public string ResolvedHubBaseUrl
        => string.IsNullOrWhiteSpace(HubBaseUrl) ? ApiBaseUrl : HubBaseUrl;
}
