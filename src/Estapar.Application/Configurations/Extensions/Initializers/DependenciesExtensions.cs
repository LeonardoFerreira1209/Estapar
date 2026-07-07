using Estapar.Application.Hubs;
using Estapar.Application.Services;
using Estapar.Domain.Contracts.Hubs;
using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Contracts.Services;
using Estapar.Infraestructure.Background;
using Estapar.Infraestructure.Data.Repositories;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estapar.Application.Configurations.Extensions.Initializers;

/// <summary>
/// Configures application dependencies, including services, repositories, facades, and infrastructure components.
/// </summary>
/// <remarks>This extension method registers various services, repositories, and other dependencies into the
/// provided  <see cref="IServiceCollection"/>. It is intended to centralize dependency injection configuration for the
/// application.  The method supports adding scoped, transient, and singleton services, including custom implementations
/// for  authentication, email providers, event handling, and OpenID Connect (OIDC) services.</remarks>
public static class DependenciesExtensions
{
    /// <summary>
    /// Configures and registers application dependencies into the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This method registers a variety of services, repositories, factories, and other dependencies
    /// required by the application. It includes scoped, transient, and singleton services, as well as custom
    /// implementations for specific interfaces.  Typical usage involves calling this method during application startup
    /// to ensure all required dependencies are properly configured.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the dependencies will be added.</param>
    /// <param name="configurations">The application configuration settings, provided as an <see cref="IConfiguration"/> instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> containing the registered dependencies.</returns>
    public static IServiceCollection ConfigureDependencies(
        this IServiceCollection services, 
        IConfiguration configurations
        )
    {
        services
            // Others
            .AddSingleton(serviceProvider => configurations)
            // Lane channel registry (singleton — shared across all hosted services and controllers)
            .AddSingleton<ILaneChannelRegistry, LaneChannelRegistry>()
            // Hosted services: initializer must be registered BEFORE the background processor
            .AddHostedService<LaneListenerInitializerService>()
            .AddHostedService<LaneListenerBackgroundService>()
            // Services
            .AddScoped<IParkService, ParkService>()
            .AddScoped<ILaneService, LaneService>()
            .AddScoped<IGarageService, GarageService>()
            .AddScoped<ILaneHubService, LaneHubService>()
            .AddScoped<IWebhookService, WebhookService>()
            // Unit of Work
            .AddScoped<IUnitOfWork, UnitOfWork>()
            // Generic repositories (fallback for unregistered entities)
            .AddScoped(typeof(IGenerictEntityCoreRepository<>), typeof(GenericEntityCoreRepository<>))
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericEntityCoreRepository<>))
            // Park-specific repositories
            .AddScoped<IParkRepository, ParkRepository>()
            .AddScoped<ILaneRepository, LaneRepository>()
            .AddScoped<IGarageRepository, GarageRepository>()
            .AddScoped<ITrafficRepository, TrafficRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<IParkedVehicleRepository, ParkedVehicleRepository>()
            .AddScoped<IPriceTableRepository, PriceTableRepository>();

        return services;
    }
}
