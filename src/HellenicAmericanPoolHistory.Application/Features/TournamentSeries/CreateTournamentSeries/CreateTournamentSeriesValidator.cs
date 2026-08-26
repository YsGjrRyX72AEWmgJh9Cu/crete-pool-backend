using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;

public sealed class CreateTournamentSeriesValidator
    : AbstractValidator<CreateTournamentSeriesCommand>
{
    public CreateTournamentSeriesValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.OrganizationId)
            .NotEmpty();
    }
}
