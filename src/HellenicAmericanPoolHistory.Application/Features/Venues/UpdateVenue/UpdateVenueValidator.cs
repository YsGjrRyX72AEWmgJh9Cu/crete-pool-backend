using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;

/// <summary>
/// Validates venue update requests.
/// </summary>
public sealed class UpdateVenueValidator
    : AbstractValidator<UpdateVenueCommand>
{
    public UpdateVenueValidator()
    {
        RuleFor(x => x.VenueId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(300);
    }
}