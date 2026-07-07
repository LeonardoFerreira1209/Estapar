using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Dtos.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estapar.Api.Controllers;

/// <summary>
/// Receives vehicle lifecycle events (ENTRY, PARKED, EXIT) from external gate systems.
/// </summary>
/// <remarks>
/// Always returns HTTP 200. Business-rule violations (e.g. garage full, vehicle already inside)
/// are recorded as error traffic records rather than returning a non-200 status.
/// </remarks>
[ApiController]
[Route("webhook")]
public class WebhookController(IWebhookService webhookService) : ControllerBase
{
    /// <summary>
    /// Processes a vehicle event webhook.
    /// </summary>
    /// <remarks>
    /// <strong>Event types:</strong>
    /// <list type="bullet">
    ///   <item><description><c>ENTRY</c> — validates garage availability and registers the vehicle entry.</description></item>
    ///   <item><description><c>PARKED</c> — acknowledges that the vehicle is physically parked (no database changes).</description></item>
    ///   <item><description><c>EXIT</c> — registers the vehicle exit, generates the billing transaction, and frees the garage.</description></item>
    /// </list>
    /// </remarks>
    /// <param name="request">The webhook payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><see cref="StatusCodes.Status200OK"/> for all processed events.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PostAsync(
        [FromBody] WebhookRequest request,
        CancellationToken cancellationToken
        )
    {
        await webhookService.ProcessAsync(
            request, 
            cancellationToken
        );

        return new ApiObjectResult(
            HttpStatusCode.OK,
            new ApiResult<object>(
                true, 
                HttpStatusCode.OK, 
                null
            )
        );
    }
}
