using Estapar.Domain.Dtos.Results;
using System.Net;

namespace Estapar.Domain.Exceptions.Base;

/// <summary>
/// Represents an exception that is thrown when a specific error condition occurs,  encapsulating an HTTP status code,
/// associated data, and a list of notifications.
/// </summary>
/// <remarks>This exception is used to provide detailed error information, including an HTTP status code,
/// additional data, and a collection of notifications that describe the error context.</remarks>
public class CustomException : BaseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomException"/> class with the specified HTTP status code, data,
    /// and notifications.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="dados">The data object containing additional information about the error.</param>
    /// <param name="notificacoes">A list of notifications detailing specific issues related to the error.</param>
    public CustomException(
        HttpStatusCode statusCode,
        object dados, List<DataNotifications> notificacoes)
    {
        Response = new ErrorResult(statusCode, dados, notificacoes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomException"/> class with the specified HTTP status code,
    /// domain error code, data, and notifications.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="errorCode">The domain-specific error code string (e.g. "USER_001").</param>
    /// <param name="dados">The data object containing additional information about the error.</param>
    /// <param name="notificacoes">A list of notifications detailing specific issues related to the error.</param>
    public CustomException(
        HttpStatusCode statusCode,
        string errorCode,
        object dados, List<DataNotifications> notificacoes)
    {
        Response = new ErrorResult(
            statusCode, 
            errorCode, 
            dados, 
            notificacoes
        );
    }
}