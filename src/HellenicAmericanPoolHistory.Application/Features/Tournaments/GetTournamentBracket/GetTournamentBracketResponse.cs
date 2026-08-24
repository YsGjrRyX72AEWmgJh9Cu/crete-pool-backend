namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;

/// <summary>
/// Represents the tournament bracket returned by the Get Tournament Bracket feature.
/// </summary>
/// <param name="TournamentId">The tournament identifier.</param>
/// <param name="TournamentName">The tournament name.</param>
/// <param name="Rounds">The tournament rounds.</param>
public sealed record GetTournamentBracketResponse(
    Guid TournamentId,
    string TournamentName,
    IReadOnlyCollection<GetTournamentBracketRoundResponse> Rounds);

/// <summary>
/// Represents a round in a tournament bracket.
/// </summary>
/// <param name="Round">The round number.</param>
/// <param name="Matches">The matches in the round.</param>
public sealed record GetTournamentBracketRoundResponse(
    int Round,
    IReadOnlyCollection<GetTournamentBracketMatchResponse> Matches);

/// <summary>
/// Represents a match in a tournament bracket.
/// </summary>
/// <param name="Id">The match identifier.</param>
/// <param name="BracketPosition">The match position within the round.</param>
/// <param name="Participant1Id">The first participation identifier.</param>
/// <param name="Participant1PlayerName">The first player's name.</param>
/// <param name="Participant2Id">The second participation identifier.</param>
/// <param name="Participant2PlayerName">The second player's name.</param>
/// <param name="WinnerParticipationId">
/// The winning participation identifier, when a result exists.
/// </param>
/// <param name="WinnerPlayerName">
/// The winner's name, when a result exists.
/// </param>
/// <param name="Participant1Score">
/// The first participant's score, when a result exists.
/// </param>
/// <param name="Participant2Score">
/// The second participant's score, when a result exists.
/// </param>
public sealed record GetTournamentBracketMatchResponse(
    Guid Id,
    int BracketPosition,
    Guid Participant1Id,
    string Participant1PlayerName,
    Guid Participant2Id,
    string Participant2PlayerName,
    Guid? WinnerParticipationId,
    string? WinnerPlayerName,
    int? Participant1Score,
    int? Participant2Score);
