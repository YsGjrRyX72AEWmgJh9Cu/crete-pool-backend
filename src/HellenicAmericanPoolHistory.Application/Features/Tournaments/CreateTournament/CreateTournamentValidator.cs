using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;

public sealed class CreateTournamentValidator
    : AbstractValidator<CreateTournamentCommand>
{
    public CreateTournamentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.TournamentType)
            .IsInEnum();

        RuleFor(x => x.BracketType)
            .IsInEnum();

        RuleFor(x => x.GameSet)
            .IsInEnum();

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate);

        RuleFor(x => x.VenueId)
            .NotEmpty();
    }
}