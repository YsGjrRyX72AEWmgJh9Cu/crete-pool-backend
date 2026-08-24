namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;

/// <summary>
/// Represents the request to advance a tournament bracket to the next round.
/// </summary>
public sealed record AdvanceTournamentBracketCommand(
    Guid TournamentId);
