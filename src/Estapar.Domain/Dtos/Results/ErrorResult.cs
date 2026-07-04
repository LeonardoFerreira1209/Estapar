using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace Estapar.Domain.Dtos.Results;

/// <summary>
/// Retorno das APIS com erro.
/// </summary>
public class ErrorResult : BaseApiResult
{
    /// <summary>
    /// Initializes a new instance of <see cref="ErrorResult"/> without an error code.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="data">Additional data to return.</param>
    /// <param name="notificacoes">List of error notifications.</param>
    public ErrorResult(
        HttpStatusCode statusCode,
        object data,
        List<DataNotifications> notificacoes = null)
        : base(statusCode, false, notificacoes)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ErrorResult"/> with a domain error code.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="errorCode">Domain-specific error code string (e.g. "USER_001").</param>
    /// <param name="data">Additional data to return.</param>
    /// <param name="notificacoes">List of error notifications.</param>
    public ErrorResult(
        HttpStatusCode statusCode,
        string errorCode,
        object data,
        List<DataNotifications> notificacoes = null)
        : base(statusCode, false, notificacoes)
    {
        ErrorCode = errorCode;
        Data = data;
    }

    /// <summary>
    /// Domain-specific error code that uniquely identifies the error type (e.g. "USER_001").
    /// </summary>
    [JsonPropertyName("errorCode")]
    [JsonProperty("errorCode")]
    public string ErrorCode { get; }

    /// <summary>
    /// Dados a serem retornados na requisição.
    /// </summary>
    [JsonProperty("data")]
    public object Data { get; }
}