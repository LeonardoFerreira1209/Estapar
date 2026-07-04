using Estapar.Domain.Dtos.Request;
using FluentValidation;

namespace Estapar.Domain.Validators;

/// <summary>
/// Validator for UpdateLaneRequest ensuring required fields are properly populated.
/// </summary>
public class UpdateLaneRequestValidator : AbstractValidator<UpdateLaneRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLaneRequestValidator"/> class.
    /// </summary>
    public UpdateLaneRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Lane name is required.")
            .MaximumLength(200)
            .WithMessage("Lane name cannot exceed 200 characters.");

        RuleFor(x => x.LaneType)
            .IsInEnum()
            .WithMessage("Lane type must be a valid value (Entry or Exit).");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Lane status must be a valid value (Active or Inactive).");
    }
}
