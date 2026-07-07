using Estapar.Domain.Dtos.Request;
using FluentValidation;

namespace Estapar.Domain.Validators;

/// <summary>
/// Validator for <see cref="UpdateParkRequest"/> ensuring all required park base fields are valid.
/// </summary>
public class UpdateParkRequestValidator : AbstractValidator<UpdateParkRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateParkRequestValidator"/> class.
    /// </summary>
    public UpdateParkRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Park name is required.")
            .MaximumLength(200)
            .WithMessage("Park name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Park description is required.")
            .MaximumLength(1000)
            .WithMessage("Park description cannot exceed 1000 characters.");
    }
}
