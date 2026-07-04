using Estapar.Domain.Dtos.Request;
using FluentValidation;

namespace Estapar.Domain.Validators;

/// <summary>
/// Validator for CreateGarageRequest ensuring required fields are properly populated.
/// </summary>
public class CreateGarageRequestValidator : AbstractValidator<CreateGarageRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGarageRequestValidator"/> class.
    /// </summary>
    public CreateGarageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Garage name is required.")
            .MaximumLength(200)
            .WithMessage("Garage name cannot exceed 200 characters.");
    }
}
