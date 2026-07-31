namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;

/// <summary>
/// Represents the player returned by the Get Player feature.
/// </summary>
/// <param name="Id">The player identifier.</param>
/// <param name="FirstName">The player's first name.</param>
/// <param name="LastName">The player's last name.</param>
/// <param name="CountryOfOrigin">The player's country of origin.</param>
/// <param name="BirthDate">The player's birth date.</param>
public sealed record GetPlayerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string CountryOfOrigin,
    DateOnly? BirthDate);