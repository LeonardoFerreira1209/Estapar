using Estapar.Application.Configurations.Extensions.Initializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Estapar.Application.Configurations.Extensions.Initializers;

/// <summary>
/// Application HealthChecks configuration class.
/// </summary>
public static class HealthCheckExtensions
{
    private static readonly string HealthCheckEndpoint = "/application/healthcheck";
    private static readonly string[] tags = ["Core", "PostgreSQL"];

    /// <summary>
    /// System HealthChecks configuration.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurations"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureHealthChecks(
        this IServiceCollection services, 
        IConfiguration configurations
        )
    {
        string connectionString = Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? configurations
                    .GetConnectionString("DataBase");

        services
           .AddHealthChecks()
           .AddNpgSql(connectionString, name: "Base de dados padrão.", tags: tags);

        return services;
    }

    /// <summary>
    /// System HealthChecks configuration.
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder application)
        => application.UseHealthChecks(HealthCheckEndpoint, new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    statusApplication = report.Status.ToString(),

                    healthChecks = report.Entries.Select(e => new
                    {
                        check = e.Key,
                        ErrorMessage = e.Value.Exception?.Message,
                        status = Enum.GetName(e.Value.Status)
                    })
                });

                context.Response.ContentType = MediaTypeNames.Application.Json;

                await context.Response.WriteAsync(result);
            }
        });
}
