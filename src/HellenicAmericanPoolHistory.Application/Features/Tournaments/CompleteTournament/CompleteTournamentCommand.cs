namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;

/// <summary>
/// Represents a request to complete a tournament.
/// </summary>
public sealed record CompleteTournamentCommand(
    Guid TournamentId);
