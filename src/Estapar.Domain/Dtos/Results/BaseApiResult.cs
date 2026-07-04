using System.Net;
using System.Text.Json.Serialization;

namespace Estapar.Domain.Dtos.Results;

/// <summary>
/// Dados a ser retornado em uma notificação do sistema.
/// </summary>
/// <param name="message"></param>
public class DataNotifications(string message)
{
    /// <summary>
    /// Mensagem da notificação.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; } = message;
}

/// <summary>
/// Classe 
/// </summary>
public abstract class BaseApiResult
{
    /// <summary>
    /// ctor recebendo o status.
    /// </summary>
    /// <param name="statusCode"></param>
    public BaseApiResult(
        HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// recebendo status, bool de sucesso, e lista de notificações. 
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="success"></param>
    /// <param name="notifications"></param>
    public BaseApiResult(HttpStatusCode statusCode,
        bool success, List<DataNotifications> notifications)
    {
        StatusCode = statusCode;
        Success = success;
        Notifications = notifications;
    }

    /// <summary>
    /// Status code.
    /// </summary>
    [JsonPropertyName("statusCode")]
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Retorna true se a requisição para API foi bem sucedida.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; }

    /// <summary>
    /// Notificações que retornam da requisição, sejam elas Sucesso, Erro, Informação.
    /// </summary>
    [JsonPropertyName("notifications")]
    public List<DataNotifications> Notifications { get; }
}
