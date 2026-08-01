namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;

/// <summary>
/// Represents a request to delete a tournament.
/// </summary>
public sealed record DeleteTournamentCommand(
    Guid TournamentId);