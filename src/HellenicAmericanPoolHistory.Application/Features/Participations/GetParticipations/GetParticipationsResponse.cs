namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;

/// <summary>
/// Represents a participation returned by the Get Participations feature.
/// </summary>
/// <param name="Id">The participation identifier.</param>
/// <param name="PlayerName">The player's full name.</param>
/// <param name="TournamentName">The tournament name.</param>
/// <param name="RegistrationDate">The registration date.</param>
/// <param name="Seed">The player's seed.</param>
/// <param name="Status">The participation status.</param>
public sealed record GetParticipationsResponse(
    Guid Id,
    string PlayerName,
    string TournamentName,
    DateOnly RegistrationDate,
    int? Seed,
    string Status);