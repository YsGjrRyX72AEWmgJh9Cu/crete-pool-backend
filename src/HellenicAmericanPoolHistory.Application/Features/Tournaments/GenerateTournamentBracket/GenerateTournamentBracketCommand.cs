namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;

/// <summary>
/// Represents the request to generate the first round of a tournament bracket.
/// </summary>
public sealed record GenerateTournamentBracketCommand(
    Guid TournamentId);
