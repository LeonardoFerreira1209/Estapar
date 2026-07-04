using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estapar.Domain.Dtos.Results;

/// <summary>
/// Represents an HTTP response with a specified status code and content value.
/// </summary>
/// <remarks>This class is a specialized implementation of <see cref="ObjectResult"/> that allows setting an HTTP
/// status code and a response body. It is typically used in APIs to return structured responses with a specific status
/// code.</remarks>
public class ApiObjectResult : ObjectResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiObjectResult"/> class with the specified HTTP status code and
    /// response value.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to associate with the result.</param>
    /// <param name="value">The response value to include in the result. This can be any object representing the response data.</param>
    public ApiObjectResult(
        HttpStatusCode statusCode,
        object value) : base(value)
    {
        StatusCode = (int)statusCode;
    }
}