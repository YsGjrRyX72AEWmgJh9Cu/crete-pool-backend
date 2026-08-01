using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;

/// <summary>
/// Validates a venue creation request.
/// </summary>
public sealed class CreateVenueValidator
    : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Address)
            .MaximumLength(250);
    }
}