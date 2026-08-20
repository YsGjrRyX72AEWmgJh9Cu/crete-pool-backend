namespace HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;

/// <summary>
/// Represents the request to record the result of a match.
/// </summary>
/// <param name="MatchId">The match identifier.</param>
/// <param name="WinnerParticipationId">The winning participation identifier.</param>
/// <param name="Participant1Score">The first participant's score.</param>
/// <param name="Participant2Score">The second participant's score.</param>
public sealed record RecordMatchResultCommand(
    Guid MatchId,
    Guid WinnerParticipationId,
    int Participant1Score,
    int Participant2Score);
