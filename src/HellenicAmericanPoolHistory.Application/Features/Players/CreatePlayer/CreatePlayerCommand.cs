namespace HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;

/// <summary>
/// Represents the request to create a new player.
/// </summary>
/// <param name="FirstName">The player's first name.</param>
/// <param name="LastName">The player's last name.</param>
/// <param name="CountryOfOrigin">The player's country of origin.</param>
/// <param name="BirthDate">The player's birth date.</param>
public sealed record CreatePlayerCommand(
    string FirstName,
    string LastName,
    string CountryOfOrigin,
    DateOnly? BirthDate);