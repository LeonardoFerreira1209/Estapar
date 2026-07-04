using Estapar.Domain.Dtos.Request;
using FluentValidation;

namespace Estapar.Domain.Validators;

/// <summary>
/// Validator for UpdateGarageRequest ensuring required fields are properly populated.
/// </summary>
public class UpdateGarageRequestValidator : AbstractValidator<UpdateGarageRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGarageRequestValidator"/> class.
    /// </summary>
    public UpdateGarageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Garage name is required.")
            .MaximumLength(200)
            .WithMessage("Garage name cannot exceed 200 characters.");
    }
}
