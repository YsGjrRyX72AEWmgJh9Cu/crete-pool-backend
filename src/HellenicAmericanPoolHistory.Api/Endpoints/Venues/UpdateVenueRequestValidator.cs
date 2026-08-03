using FluentValidation;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

/// <summary>
/// Validates update venue requests.
/// </summary>
public sealed class UpdateVenueRequestValidator
    : AbstractValidator<UpdateVenueRequest>
{
    public UpdateVenueRequestValidator()
    {
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