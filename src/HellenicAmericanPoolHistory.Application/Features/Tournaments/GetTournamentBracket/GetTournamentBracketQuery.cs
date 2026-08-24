namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;

/// <summary>
/// Represents a request to retrieve a tournament bracket.
/// </summary>
/// <param name="TournamentId">The tournament identifier.</param>
public sealed record GetTournamentBracketQuery(
    Guid TournamentId);
