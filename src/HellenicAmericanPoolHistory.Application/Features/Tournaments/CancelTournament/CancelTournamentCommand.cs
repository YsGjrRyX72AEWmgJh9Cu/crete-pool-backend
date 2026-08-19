namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;

/// <summary>
/// Represents a request to cancel a tournament.
/// </summary>
public sealed record CancelTournamentCommand(
    Guid TournamentId);
