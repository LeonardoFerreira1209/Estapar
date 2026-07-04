using Estapar.Application.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Estapar.Application.Configurations.Extensions.Initializers;

/// <summary>
/// Provides extension methods for configuring Swagger services and middleware in an ASP.NET Core application.
/// </summary>
/// <remarks>This static class contains methods to set up Swagger for API documentation, including versioning and
/// security configurations. It allows the integration of Swagger services and middleware using configuration settings
/// specified in the application's configuration files.</remarks>
public static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger services for the application with specified settings.
    /// </summary>
    /// <remarks>This method sets up Swagger with API versioning, security definitions for JWT Bearer
    /// authentication, and additional documentation filters. It retrieves configuration values for API version, title,
    /// and description from the provided <paramref name="configurations"/>.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the Swagger services are added.</param>
    /// <param name="configurations">The <see cref="IConfiguration"/> instance containing Swagger configuration settings.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> with Swagger services configured.</returns>
    public static IServiceCollection ConfigureSwagger(
        this IServiceCollection services, 
        IConfiguration configurations
        )
    {
        var apiVersion = configurations.GetValue<string>("SwaggerInfo:ApiVersion");
        var ApiTitle = configurations.GetValue<string>("SwaggerInfo:ApiTitle");
        var description = configurations.GetValue<string>("SwaggerInfo:Description");

        services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();

            swagger.SwaggerDoc(apiVersion, new OpenApiInfo
            {
                Version = apiVersion,
                Title = $"Estapar - {apiVersion}",
                Description = description,
            });

            swagger.DocumentFilter<HealthCheckSwagger>();
        });

        return services;
    }

    /// <summary>
    /// Configures Swagger middleware for the application with specified settings.
    /// </summary>
    /// <remarks>This method sets up the Swagger middleware to serve the generated Swagger JSON and the
    /// Swagger UI. The API version is retrieved from the configuration and used to define the Swagger endpoint
    /// route.</remarks>
    /// <param name="application">The <see cref="IApplicationBuilder"/> instance to configure.</param>
    /// <param name="configurations">The <see cref="IConfiguration"/> containing Swagger configuration settings.</param>
    /// <returns>The configured <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder UseSwaggerConfigurations(
        this IApplicationBuilder application, 
        IConfiguration configurations
        )
    {
        var apiVersion = configurations.GetValue<string>("SwaggerInfo:ApiVersion");

        application.UseSwagger(options =>
        {
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        application
            .UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"{apiVersion}");
            });

        return application;
    }
}
