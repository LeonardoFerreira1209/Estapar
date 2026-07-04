using Estapar.Application.Services;
using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Contracts.Services;
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
            // Services
            .AddScoped<IParkService, ParkService>()
            // Unit of Work
            .AddScoped<IUnitOfWork, UnitOfWork>()
            // Generic repositories (fallback for unregistered entities)
            .AddScoped(typeof(IGenerictEntityCoreRepository<>), typeof(GenericEntityCoreRepository<>))
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericEntityCoreRepository<>))
            // Park-specific repositories
            .AddScoped<IParkRepository, ParkRepository>()
            .AddScoped<ILaneRepository, LaneRepository>()
            .AddScoped<IGarageRepository, GarageRepository>();

            // Other specific repositories
            //.AddScoped<IRealmRepository, RealmRepository>()
            //.AddScoped<IClientRepository, ClientRepository>()
            //.AddScoped<IClientAllowedScopeRepository, ClientAllowedScopeRepository>()
            //.AddScoped<IClientAllowedGrantTypeRepository, ClientAllowedGrantTypeRepository>()
            //.AddScoped<IClientAllowedPkceMethodRepository, ClientAllowedPkceMethodRepository>()
            //.AddScoped<IClientAllowedLoginFlowRepository, ClientAllowedLoginFlowRepository>()
            //.AddScoped<IClientConsumerRepository, ClientConsumerRepository>()
            //.AddScoped<IFeatureFlagsRepository, FeatureFlagsRepository>()
            //.AddScoped<IClientConfigurationRepository, ClientConfigurationRepository>()
            //.AddScoped<IClientIdentityConfigurationRepository, ClientIdentityConfigurationRepository>()
            //.AddScoped<IUserIdentityConfigurationRepository, UserIdentityConfigurationRepository>()
            //.AddScoped<IPasswordIdentityConfigurationRepository, PasswordIdentityConfigurationRepository>()
            //.AddScoped<ILockoutIdentityConfigurationRepository, LockoutIdentityConfigurationRepository>()
            //.AddScoped<IClientEmailConfigurationRepository, ClientEmailConfigurationRepository>()
            //.AddScoped<IClientTokenConfigurationRepository, ClientTokenConfigurationRepository>()
            //.AddScoped<ISendGridConfigurationRepository, SendGridConfigurationRepository>()
            //.AddScoped<IEventRepository, EventRepository>()
            //.AddScoped<IPlanRepository, PlanRepository>()
            //.AddScoped<IAccessLogRepository, AccessLogRepository>()
            //.AddScoped<IPlanRoleRepository, PlanRoleRepository>()
            //.AddScoped<IRoleRepository, RoleRepository>()
            //.AddScoped<ISecretRepository, SecretRepository>()
            //.AddScoped<ISubscriptionRepository, SubscriptionRepository>()
            //.AddScoped<IFeatureFlagsRepository, FeatureFlagsRepository>()
            //.AddScoped<ICustomUserStore<UserEntity>, CustomUserStore<UserEntity>>()
            //.AddScoped<ICustomSignManager<UserEntity>, CustomSignInManager<UserEntity>>()
            //.AddScoped<ICustomUserManager<UserEntity>, CustomUserManager<UserEntity>>()
            //.AddScoped<IScopeRepository, ScopeRepository>()
            //.AddScoped<IGrantTypeRepository, GrantTypeRepository>()
            //.AddScoped<IPkceMethodRepository, PkceMethodRepository>()
            //.AddScoped<ILoginFlowRepository, LoginFlowRepository>()
            //.AddScoped<IUserSystemRepository, UserSystemRepository>()
            //.AddScoped<IUserRoleRepository, UserRoleRepository>()
            //.AddScoped<IHybridCacheRepository, HybridCacheRepository>()
           


        return services;
    }
}
