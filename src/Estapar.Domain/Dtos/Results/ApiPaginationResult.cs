using System.Net;

namespace Estapar.Domain.Dtos.Results;

/// <summary>
/// Retorno das APIS paginada.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiPaginationResult<T>
    : BaseApiResult where T : class
{
    /// <summary>
    /// Construtor simples
    /// </summary>
    /// <param name="statusCode"></param>
    public ApiPaginationResult(HttpStatusCode statusCode)
        : base(statusCode) { }

    /// <summary>
    /// Construtor com recebimento de dados parcial.
    /// </summary>
    /// <param name="success"></param>
    /// <param name="statusCode"></param>
    /// <param name="notifications"></param>
    public ApiPaginationResult(bool success, HttpStatusCode statusCode,
        List<DataNotifications> notifications = null)
            : base(statusCode, success, notifications)
    {
    }

    /// <summary>
    /// Construtor que recebe todos os itens.
    /// </summary>
    /// <param name="success"></param>
    /// <param name="statusCode"></param>
    /// <param name="paginatedResponse"></param>
    /// <param name="notifications"></param>
    public ApiPaginationResult(bool success, HttpStatusCode statusCode,
        PaginatedResult<T> paginatedResponse, List<DataNotifications> notifications = null)
            : base(statusCode, success, notifications)
    {
        Pagination = paginatedResponse;
    }

    /// <summary>
    /// Dados da paginação.
    /// </summary>
    public PaginatedResult<T> Pagination { get; set; }
}
