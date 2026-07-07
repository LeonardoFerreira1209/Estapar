using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Exceptions.Base;
using System.Net;

namespace Estapar.Domain.Exceptions;

/// <summary>
/// Represents errors specific to the Garage domain.
/// </summary>
public class GarageException : CustomException
{
    /// <summary>
    /// Initializes a new instance of <see cref="GarageException"/> with the specified HTTP status code,
    /// error code, data, and notifications.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="errorCode">The domain-specific error code string (e.g. "GARAGE_NOT_FOUND").</param>
    /// <param name="dados">Additional data to include in the error response. Can be null.</param>
    /// <param name="notificacoes">A list of notifications detailing the error.</param>
    private GarageException(
        HttpStatusCode statusCode,
        string errorCode,
        object dados,
        List<DataNotifications> notificacoes)
        : base(statusCode, errorCode, dados, notificacoes) { }

    /// <summary>
    /// Creates a <see cref="GarageException"/> for when a garage with the given identifier is not found.
    /// </summary>
    /// <param name="id">The identifier of the garage that was not found.</param>
    /// <returns>A <see cref="GarageException"/> with HTTP 404 Not Found.</returns>
    public static GarageException NotFound(Guid id) =>
        new(
            HttpStatusCode.NotFound,
            "GARAGE_NOT_FOUND",
            null,
            [new DataNotifications($"Garage with ID {id} was not found.")]
        );

    /// <summary>
    /// Creates a <see cref="GarageException"/> for garage request validation errors.
    /// </summary>
    /// <param name="notifications">The list of validation error notifications.</param>
    /// <returns>A <see cref="GarageException"/> with HTTP 422 Unprocessable Entity.</returns>
    public static GarageException ValidationError(List<DataNotifications> notifications) =>
        new(
            HttpStatusCode.UnprocessableEntity,
            "GARAGE_VALIDATION_ERROR",
            null,
            notifications
        );
}
