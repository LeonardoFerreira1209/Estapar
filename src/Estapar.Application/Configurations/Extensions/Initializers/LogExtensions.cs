using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Estapar.Application.Configurations.Extensions.Initializers;

/// <summary>
/// Provides extension methods for configuring logging, telemetry, and Application Insights in an ASP.NET Core
/// application.
/// </summary>
/// <remarks>This static class contains methods to configure Serilog, telemetry, and Application Insights for
/// dependency injection in an ASP.NET Core application. These methods are designed to integrate logging and monitoring
/// capabilities into the application, leveraging Serilog and Azure Application Insights.</remarks>
public static class LogExtensions
{
    /// <summary>
    /// Configuração de Logs do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureSerilog(
        this IServiceCollection services
        )
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .WriteTo.Console()
                .CreateLogger();

        return services;
    }
}
