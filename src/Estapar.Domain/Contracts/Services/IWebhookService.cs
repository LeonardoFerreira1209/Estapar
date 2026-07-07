using Estapar.Domain.Dtos.Request;

namespace Estapar.Domain.Contracts.Services;

/// <summary>
/// Service interface for processing incoming webhook events (ENTRY, PARKED, EXIT).
/// </summary>
public interface IWebhookService
{
    /// <summary>
    /// Processes a webhook event based on its <see cref="WebhookRequest.EventType"/>.
    /// </summary>
    /// <param name="request">The webhook request payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task ProcessAsync(WebhookRequest request, CancellationToken cancellationToken = default);
}
