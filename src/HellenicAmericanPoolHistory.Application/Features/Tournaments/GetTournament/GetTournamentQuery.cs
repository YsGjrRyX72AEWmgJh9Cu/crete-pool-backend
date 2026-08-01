namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;

/// <summary>
/// Represents a request to retrieve a tournament.
/// </summary>
public sealed record GetTournamentQuery(Guid TournamentId);