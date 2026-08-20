using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;

public interface IRecordMatchResultPort
{
    Task RecordAsync(
        MatchId matchId,
        ParticipationId winnerParticipationId,
        int participant1Score,
        int participant2Score,
        CancellationToken cancellationToken);
}
