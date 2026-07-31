using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;

/// <summary>
/// Represents the request to update an existing player.
/// </summary>
/// <param name="Id">The identifier of the player.</param>
/// <param name="FirstName">The player's first name.</param>
/// <param name="LastName">The player's last name.</param>
/// <param name="CountryOfOrigin">The player's country of origin.</param>
/// <param name="BirthDate">The player's birth date.</param>
public sealed record UpdatePlayerCommand(
    PlayerId Id,
    string FirstName,
    string LastName,
    string CountryOfOrigin,
    DateOnly? BirthDate);