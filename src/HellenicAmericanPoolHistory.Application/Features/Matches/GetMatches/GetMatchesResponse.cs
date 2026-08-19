namespace HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;

/// <summary>
/// Represents a match returned by the Get Matches feature.
/// </summary>
/// <param name="Id">The match identifier.</param>
/// <param name="TournamentId">The tournament identifier.</param>
/// <param name="TournamentName">The tournament name.</param>
/// <param name="Participant1Id">The first participation identifier.</param>
/// <param name="Participant1PlayerName">The first player's name.</param>
/// <param name="Participant2Id">The second participation identifier.</param>
/// <param name="Participant2PlayerName">The second player's name.</param>
/// <param name="WinnerParticipationId">The winning participation identifier.</param>
/// <param name="WinnerPlayerName">The winner's name.</param>
/// <param name="Participant1Score">The first participant's score.</param>
/// <param name="Participant2Score">The second participant's score.</param>
public sealed record GetMatchesResponse(
    Guid Id,
    Guid TournamentId,
    string TournamentName,
    Guid Participant1Id,
    string Participant1PlayerName,
    Guid Participant2Id,
    string Participant2PlayerName,
    Guid WinnerParticipationId,
    string WinnerPlayerName,
    int Participant1Score,
    int Participant2Score);
