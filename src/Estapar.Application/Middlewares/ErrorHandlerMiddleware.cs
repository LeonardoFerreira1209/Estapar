using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Exceptions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Estapar.Application.Middlewares;

/// <summary>
/// Middleware that handles exceptions occurring during the processing of HTTP requests.
/// </summary>
/// <remarks>This middleware intercepts unhandled exceptions in the request pipeline and generates a structured
/// JSON response based on the exception type. It ensures that the application does not expose sensitive information
/// while providing meaningful error details to the client.</remarks>
/// <param name="requestDelegate">The next middleware in the pipeline.</param>
/// <param name="environment">The hosting environment to determine error detail exposure.</param>
public class ErrorHandlerMiddleware(
    RequestDelegate requestDelegate,
    IHostEnvironment environment)
{
    /// <summary>
    /// Invokes the specified HTTP request delegate asynchronously, processing the provided HTTP context.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> representing the current HTTP request and response.</param>
    /// <returns>A task that represents the asynchronous operation of invoking the request delegate.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles an exception by generating an appropriate HTTP response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> representing the current HTTP request and response.</param>
    /// <param name="exception">The <see cref="Exception"/> to be handled.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation of writing the response.</returns>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        Log.Error(
            exception,
            "[LOG ERROR] - Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
            context.TraceIdentifier,
            context.Request.Path,
            context.Request.Method
        );

        var (statusCode, json) = GenerateResponse(
            exception, 
            context.TraceIdentifier
        );

        context
            .Response
            .StatusCode = (int)statusCode;

        return context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Generates an HTTP response based on the provided exception.
    /// Only exposes detailed error information in development environment.
    /// </summary>
    /// <param name="exception">The exception to process.</param>
    /// <param name="traceId">The trace identifier for correlation.</param>
    /// <returns>A tuple containing the HTTP status code and a JSON string representation of the response.</returns>
    private (HttpStatusCode statusCode, string json) GenerateResponse(
        Exception exception,
        string traceId
        )
    {
        if (exception is BaseException customEx)
        {
            return (
                customEx.Response.StatusCode,
                JsonSerializer.Serialize(customEx.Response)
            );
        }

        var isDevelopment = environment.IsDevelopment();

        var errorMessage = isDevelopment
            ? exception.Message
            : "An unexpected error occurred. Please try again later.";

        var notification = isDevelopment
            ? new DataNotifications(errorMessage)
            : new DataNotifications($"{errorMessage} (Reference: {traceId})");

        var response = new ErrorResult(
            HttpStatusCode.InternalServerError,
            isDevelopment ? (object)new { traceId } : null,
            [notification]
        );

        return (HttpStatusCode.InternalServerError, JsonSerializer.Serialize(response));
    }
}

