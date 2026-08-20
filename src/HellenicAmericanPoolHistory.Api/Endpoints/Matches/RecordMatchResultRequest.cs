namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

/// <summary>
/// Represents the API request to record the result of a match.
/// </summary>
/// <param name="WinnerParticipationId">
/// The winning participation identifier.
/// </param>
/// <param name="Participant1Score">
/// The first participant's score.
/// </param>
/// <param name="Participant2Score">
/// The second participant's score.
/// </param>
public sealed record RecordMatchResultRequest(
    Guid WinnerParticipationId,
    int Participant1Score,
    int Participant2Score);
