using FluentValidation;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

public sealed class RecordMatchResultRequestValidator
    : AbstractValidator<RecordMatchResultRequest>
{
    public RecordMatchResultRequestValidator()
    {
        RuleFor(x => x.WinnerParticipationId)
            .NotEmpty();

        RuleFor(x => x.Participant1Score)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Participant2Score)
            .GreaterThanOrEqualTo(0);
    }
}
