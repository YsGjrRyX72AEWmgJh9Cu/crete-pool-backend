using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;

public sealed class RecordMatchResultValidator
    : AbstractValidator<RecordMatchResultCommand>
{
    public RecordMatchResultValidator()
    {
        RuleFor(x => x.MatchId)
            .NotEmpty();

        RuleFor(x => x.WinnerParticipationId)
            .NotEmpty();

        RuleFor(x => x.Participant1Score)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Participant2Score)
            .GreaterThanOrEqualTo(0);
    }
}
