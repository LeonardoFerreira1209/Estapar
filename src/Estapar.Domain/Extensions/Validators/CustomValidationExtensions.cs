using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Exceptions.Base;
using FluentValidation.Results;
using System.Net;

namespace Estapar.Domain.Extensions.Validators;

/// <summary>
/// Provides extension methods for handling validation errors in a <see cref="ValidationResult"/>.
/// </summary>
/// <remarks>This class includes methods to process validation errors and throw custom exceptions with detailed
/// error notifications.</remarks>
public static class CustomValidationExtensions
{
    /// <summary>
    /// Throws a custom exception containing validation errors from the specified <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="validationResult">The validation result containing the errors to process. Cannot be null.</param>
    /// <param name="dados">Optional additional data to include in the exception. Can be null.</param>
    /// <returns></returns>
    /// <exception cref="CustomException">Thrown with a <see cref="HttpStatusCode.BadRequest"/> status, including the validation errors and optional
    /// additional data.</exception>
    public static Task GetValidationErrors(
        this ValidationResult validationResult, 
        object dados = null
        )
    {
        var notificacoes = new List<DataNotifications>();

        foreach (var error in validationResult.Errors) 
            notificacoes.Add(
                new DataNotifications(error.ErrorMessage)
            );

        throw new CustomException(
            HttpStatusCode.BadRequest, 
            dados, 
            notificacoes
        );
    }
}