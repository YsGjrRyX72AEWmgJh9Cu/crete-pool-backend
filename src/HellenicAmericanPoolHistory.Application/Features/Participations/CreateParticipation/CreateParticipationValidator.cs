using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;

public sealed class CreateParticipationValidator
    : AbstractValidator<CreateParticipationCommand>
{
    public CreateParticipationValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotEmpty();

        RuleFor(x => x.TournamentId)
            .NotEmpty();

        RuleFor(x => x.RegistrationDate)
            .NotEmpty();

        RuleFor(x => x.Seed)
            .GreaterThan(0)
            .When(x => x.Seed.HasValue);
    }
}