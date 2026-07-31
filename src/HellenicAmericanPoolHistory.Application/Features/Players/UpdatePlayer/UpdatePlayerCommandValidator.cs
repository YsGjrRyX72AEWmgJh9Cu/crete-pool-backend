using FluentValidation;
using HellenicAmericanPoolHistory.Application.Common.Validation;

namespace HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;

public sealed class UpdatePlayerCommandValidator
    : AbstractValidator<UpdatePlayerCommand>
{
    public UpdatePlayerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(ValidationConstants.NameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(ValidationConstants.NameMaxLength);

        RuleFor(x => x.CountryOfOrigin)
            .NotEmpty()
            .MaximumLength(ValidationConstants.CountryMaxLength);

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.BirthDate.HasValue);
    }
}