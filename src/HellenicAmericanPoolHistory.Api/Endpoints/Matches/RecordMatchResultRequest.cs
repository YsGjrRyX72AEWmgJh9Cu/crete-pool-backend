namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

public sealed record RecordMatchResultRequest(
    Guid WinnerParticipationId,
    int Participant1Score,
    int Participant2Score);
