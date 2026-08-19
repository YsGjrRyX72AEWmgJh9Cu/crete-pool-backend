using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

public sealed class CreateMatchValidator
    : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchValidator()
    {
        RuleFor(x => x.TournamentId)
            .NotEmpty();

        RuleFor(x => x.Participant1Id)
            .NotEmpty();

        RuleFor(x => x.Participant2Id)
            .NotEmpty();

        RuleFor(x => x.WinnerParticipationId)
            .NotEmpty();

        RuleFor(x => x.Participant1Score)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Participant2Score)
            .GreaterThanOrEqualTo(0);
    }
}
