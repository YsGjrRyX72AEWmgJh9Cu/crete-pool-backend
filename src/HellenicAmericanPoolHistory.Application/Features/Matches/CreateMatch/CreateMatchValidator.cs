using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

public sealed class CreateMatchValidator
    : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchValidator()
    {
        RuleFor(x => x.TournamentId)
            .NotEmpty();

        RuleFor(x => x.Round)
            .GreaterThan(0);

        RuleFor(x => x.BracketPosition)
            .GreaterThan(0);

        RuleFor(x => x.Participant1Id)
            .NotEmpty();

        RuleFor(x => x.Participant2Id)
            .NotEmpty();

        RuleFor(x => x.Participant1Score)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Participant1Score.HasValue);

        RuleFor(x => x.Participant2Score)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Participant2Score.HasValue);

        RuleFor(x => x)
            .Must(x =>
                (!x.WinnerParticipationId.HasValue &&
                 !x.Participant1Score.HasValue &&
                 !x.Participant2Score.HasValue)
                ||
                (x.WinnerParticipationId.HasValue &&
                 x.Participant1Score.HasValue &&
                 x.Participant2Score.HasValue))
            .WithMessage(
                "Winner and scores must either all be provided or all be omitted.");
    }
}
