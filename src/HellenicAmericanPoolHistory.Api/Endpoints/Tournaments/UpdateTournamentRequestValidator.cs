using FluentValidation;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public sealed class UpdateTournamentRequestValidator
    : AbstractValidator<UpdateTournamentRequest>
{
    public UpdateTournamentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate);

        RuleFor(x => x.VenueId)
            .NotEmpty();
    }
}