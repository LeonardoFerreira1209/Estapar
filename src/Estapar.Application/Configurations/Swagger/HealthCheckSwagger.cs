using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Estapar.Application.Configurations.Swagger;

/// <summary>
/// Adds a health check endpoint to the Swagger documentation.
/// </summary>
/// <remarks>This class implements the <see cref="IDocumentFilter"/> interface to modify the Swagger document by
/// adding a predefined health check endpoint. The endpoint is added at the path specified by <see
/// cref="HealthCheckEndpoint"/> and provides information about the application's health status.</remarks>
public class HealthCheckSwagger : IDocumentFilter
{
    public static readonly string HealthCheckEndpoint = "/application/healthcheck";

    /// <summary>
    /// Adds a health check endpoint to the provided OpenAPI document.
    /// </summary>
    /// <remarks>This method adds a predefined health check endpoint to the OpenAPI document. The endpoint
    /// includes a GET operation with responses indicating the health status of the application: <list type="bullet">
    /// <item><description>HTTP 200: Indicates the application is healthy.</description></item> <item><description>HTTP
    /// 503: Indicates the application is unhealthy.</description></item> </list> The endpoint is tagged as
    /// "Health-Check" for easy identification in the API documentation.</remarks>
    /// <param name="swaggerDoc">The OpenAPI document to which the health check endpoint will be added.</param>
    /// <param name="context">The context for the document filter, providing additional metadata or services if needed.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var pathItem = new OpenApiPathItem();

        pathItem.Operations.Add(OperationType.Get, new OpenApiOperation
        {
            OperationId = "HeathCheck",
            Tags = [new OpenApiTag { Name = "Health-Check" }],
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse { Description = "Healthy" },
                ["503"] = new OpenApiResponse { Description = "Unhealthy" }
            }
        });

        swaggerDoc.Paths.Add(HealthCheckEndpoint, pathItem);
    }
}
