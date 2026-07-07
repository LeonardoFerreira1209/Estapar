using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Exceptions.Base;
using System.Net;

namespace Estapar.Domain.Exceptions;

/// <summary>
/// Represents errors specific to the Lane domain.
/// </summary>
public class LaneException : CustomException
{
    /// <summary>
    /// Initializes a new instance of <see cref="LaneException"/> with the specified HTTP status code,
    /// error code, data, and notifications.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="errorCode">The domain-specific error code string (e.g. "LANE_NOT_FOUND").</param>
    /// <param name="dados">Additional data to include in the error response. Can be null.</param>
    /// <param name="notificacoes">A list of notifications detailing the error.</param>
    private LaneException(
        HttpStatusCode statusCode,
        string errorCode,
        object dados,
        List<DataNotifications> notificacoes)
        : base(statusCode, errorCode, dados, notificacoes) { }

    /// <summary>
    /// Creates a <see cref="LaneException"/> for when a lane with the given identifier is not found.
    /// </summary>
    /// <param name="id">The identifier of the lane that was not found.</param>
    /// <returns>A <see cref="LaneException"/> with HTTP 404 Not Found.</returns>
    public static LaneException NotFound(Guid id) =>
        new(
            HttpStatusCode.NotFound,
            "LANE_NOT_FOUND",
            null,
            [new DataNotifications($"Lane with ID {id} was not found.")]
        );

    /// <summary>
    /// Creates a <see cref="LaneException"/> for lane request validation errors.
    /// </summary>
    /// <param name="notifications">The list of validation error notifications.</param>
    /// <returns>A <see cref="LaneException"/> with HTTP 422 Unprocessable Entity.</returns>
    public static LaneException ValidationError(List<DataNotifications> notifications) =>
        new(
            HttpStatusCode.UnprocessableEntity,
            "LANE_VALIDATION_ERROR",
            null,
            notifications
        );
}
