using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;

/// <summary>
/// Validates the request to create a player.
/// </summary>
public sealed class CreatePlayerValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CountryOfOrigin)
            .NotEmpty()
            .MaximumLength(100);
    }
}