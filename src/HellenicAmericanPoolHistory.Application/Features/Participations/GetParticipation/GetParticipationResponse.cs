namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;

/// <summary>
/// Represents the participation returned by the Get Participation feature.
/// </summary>
/// <param name="Id">The participation identifier.</param>
/// <param name="PlayerId">The player identifier.</param>
/// <param name="PlayerName">The player's full name.</param>
/// <param name="TournamentId">The tournament identifier.</param>
/// <param name="TournamentName">The tournament name.</param>
/// <param name="RegistrationDate">The registration date.</param>
/// <param name="Seed">The player's seed.</param>
/// <param name="Status">The participation status.</param>
public sealed record GetParticipationResponse(
    Guid Id,
    Guid PlayerId,
    string PlayerName,
    Guid TournamentId,
    string TournamentName,
    DateOnly RegistrationDate,
    int? Seed,
    string Status);