using Estapar.Infraestructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Estapar.Application.Configurations.Extensions.Initializers;

/// <summary>
/// Provides extension methods for configuring the application's database services.
/// </summary>
public static class DataBaseExtensions
{
    /// <summary>
    /// Configures the AuthIoContext database context for dependency injection using PostgreSQL and adds it to the
    /// service collection.
    /// </summary>
    /// <remarks>The connection string is obtained from the POSTGRES_DATABASE environment variable if set;
    /// otherwise, it is retrieved from the configuration's 'DataBase' connection string. The context is registered with
    /// a scoped lifetime and uses lazy loading proxies. Logging is configured at the warning level.</remarks>
    /// <param name="services">The service collection to which the database context will be added. Cannot be null.</param>
    /// <param name="configurations">The application configuration used to retrieve the database connection string. Cannot be null.</param>
    /// <returns>The same IServiceCollection instance with the AuthIoContext configured for use with PostgreSQL.</returns>
    public static IServiceCollection ConfigureDataBase(
        this IServiceCollection services, 
        IConfiguration configurations
        )
        => services.AddDbContext<EstaparContext>(options =>
        {
            string connectionString = Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? configurations
                    .GetConnectionString("DataBase");

            options
                .UseNpgsql(connectionString)
                .UseLazyLoadingProxies()
                .LogTo(Console.WriteLine, LogLevel.Warning);

        }, ServiceLifetime.Scoped);

    /// <summary>
    /// Applies any pending database migrations to ensure the database schema is up to date.
    /// </summary>
    /// <remarks>This method creates a scoped service provider to resolve the database context and applies all
    /// pending migrations. If an error occurs during the migration process, the exception is logged and rethrown.
    /// Ensure that the application has the necessary permissions to modify the database schema.</remarks>
    /// <param name="application"></param>
    public static void ExecuteMigration(this WebApplication application)
    {
        using var scope =
            application.Services.CreateScope();

        var dbContext =
            scope
            .ServiceProvider
            .GetRequiredService<EstaparContext>();

        dbContext.Database.Migrate();
    }
}
