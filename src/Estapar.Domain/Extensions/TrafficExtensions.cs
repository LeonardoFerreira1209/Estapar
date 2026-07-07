using Estapar.Domain.Builders;
using Estapar.Domain.Dtos.Request;
using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Traffic;

namespace Estapar.Domain.Extensions;

/// <summary>
/// Extension methods for creating TrafficEntity instances from webhook requests.
/// </summary>
public static class TrafficExtensions
{
    /// <summary>
    /// Converts a <see cref="WebhookRequest"/> into a <see cref="TrafficEntity"/>, recording the outcome of a traffic attempt.
    /// </summary>
    /// <param name="request">The webhook request containing the vehicle license plate.</param>
    /// <param name="date">The date and time of the traffic attempt.</param>
    /// <param name="laneId">The identifier of the lane where the attempt occurred.</param>
    /// <param name="action">The action type (entry, park, or exit).</param>
    /// <param name="success">True if the attempt was successful; otherwise, false.</param>
    /// <param name="error">The error that occurred during the attempt, if any.</param>
    /// <param name="balance">
    /// The dynamic price locked in for this attempt (e.g. the entry price calculated from occupancy).
    /// Defaults to zero for attempts that do not carry a price (park/exit/error attempts).
    /// </param>
    /// <returns>A new <see cref="TrafficEntity"/> instance.</returns>
    public static TrafficEntity ToTrafficEntity(
        this WebhookRequest request,
        DateTime date,
        Guid laneId,
        TrafficAction action,
        bool success,
        TrafficError error = TrafficError.None,
        decimal balance = 0m
        )
        => new TrafficEntityBuilder()
            .AddLicensePlate(request.LicensePlate)
            .AddDate(date)
            .AddLaneId(laneId)
            .AddAction(action)
            .AddSuccess(success)
            .AddBalance(balance)
            .AddError(error)
            .Builder();
}
