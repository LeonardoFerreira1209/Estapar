using Estapar.Domain.Dtos.Request;
using FluentValidation;

namespace Estapar.Domain.Validators;

/// <summary>
/// Validator for CreateParkRequest ensuring all required park fields and nested collections are valid.
/// </summary>
public class CreateParkRequestValidator : AbstractValidator<CreateParkRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateParkRequestValidator"/> class.
    /// </summary>
    public CreateParkRequestValidator()
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

        RuleForEach(x => x.Lanes)
            .SetValidator(new CreateLaneRequestValidator());

        RuleForEach(x => x.Garages)
            .SetValidator(new CreateGarageRequestValidator());
    }
}
