using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;

/// <summary>
/// Validates an update participation command.
/// </summary>
public sealed class UpdateParticipationCommandValidator
    : AbstractValidator<UpdateParticipationCommand>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UpdateParticipationCommandValidator"/> class.
    /// </summary>
    public UpdateParticipationCommandValidator()
    {
        RuleFor(command => command.ParticipationId)
            .NotEmpty()
            .WithMessage("Participation identifier is required.");

        RuleFor(command => command.Seed)
            .GreaterThan(0)
            .When(command => command.Seed.HasValue)
            .WithMessage("Seed must be greater than zero.");

        RuleFor(command => command.Status)
            .IsInEnum()
            .WithMessage("Participation status is invalid.");
    }
}