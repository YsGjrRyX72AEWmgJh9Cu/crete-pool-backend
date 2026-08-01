using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;

/// <summary>
/// Validates tournament update requests.
/// </summary>
public sealed class UpdateTournamentValidator
    : AbstractValidator<UpdateTournamentCommand>
{
    public UpdateTournamentValidator()
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