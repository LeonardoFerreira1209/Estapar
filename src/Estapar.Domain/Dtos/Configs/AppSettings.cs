namespace Estapar.Domain.Dtos.Configs;

/// <summary>
/// Represents the application settings configuration, typically loaded from a configuration file.
/// </summary>
/// <remarks>This class encapsulates various configuration sections required by the application, such as 
/// connection strings, authentication settings, retry policies, and more. Each property corresponds  to a specific
/// configuration section and provides strongly-typed access to its settings.</remarks>
public sealed class AppSettings
{
    /// <summary>
    /// Gets or sets the connection strings used to configure database or service connections.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; set; }

    /// <summary>
    /// Gets or sets the Swagger configuration information for the API.
    /// </summary>
    public SwaggerInfo SwaggerInfo { get; set; }
}

/// <summary>
/// Represents a collection of connection strings used to configure access to various resources.
/// </summary>
/// <remarks>This class provides properties for storing connection strings to resources such as databases.
/// These connection strings are typically used for application configuration and should be securely managed.</remarks>
public sealed class ConnectionStrings
{
    public string DataBase { get; set; }
}

/// <summary>
/// Represents metadata information about the API, including its description, version, and related resources.
/// </summary>
/// <remarks>This class is typically used to provide descriptive information about the API for documentation
/// purposes, such as generating Swagger/OpenAPI specifications.</remarks>
public sealed class SwaggerInfo
{
    public string ApiDescription { get; set; }
    public string ApiVersion { get; set; }
    public string UriMyGit { get; set; }
}