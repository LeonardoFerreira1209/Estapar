using Estapar.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Estapar.Application.Configurations.Extensions.Initializers;

/// <summary>
/// Provides extension methods for configuring middleware in the application's request pipeline.
/// </summary>
/// <remarks>This class contains methods that simplify the setup of middleware components in an ASP.NET Core
/// application. Use these methods to configure middleware in a fluent and consistent manner.</remarks>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Configures the application to use centralized error handling middleware for capturing and processing unhandled
    /// exceptions.
    /// </summary>
    /// <remarks>This middleware ensures that unhandled exceptions are intercepted and handled consistently,
    /// providing standardized error responses to clients and improving overall error management across the
    /// application.</remarks>
    /// <param name="builder">The application builder used to configure the HTTP request pipeline.</param>
    /// <returns>The same instance of the application builder, enabling method chaining.</returns>
    public static IApplicationBuilder UseErrorHandlerMiddleware(
      this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
