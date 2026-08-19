namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;

/// <summary>
/// Represents a request to start a tournament.
/// </summary>
public sealed record StartTournamentCommand(
    Guid TournamentId);