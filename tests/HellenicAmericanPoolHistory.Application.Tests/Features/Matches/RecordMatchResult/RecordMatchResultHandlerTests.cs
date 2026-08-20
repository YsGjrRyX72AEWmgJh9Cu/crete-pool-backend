using HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Matches.RecordMatchResult;

public sealed class RecordMatchResultHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Record_Result_Through_Port()
    {
        var port = new FakeRecordMatchResultPort();
        var handler = new RecordMatchResultHandler(port);

        var matchId = Guid.NewGuid();
        var winnerParticipationId = Guid.NewGuid();

        var command = new RecordMatchResultCommand(
            matchId,
            winnerParticipationId,
            5,
            3);

        await handler.HandleAsync(command);

        Assert.Equal(
            matchId,
            port.MatchId!.Value.Value);

        Assert.Equal(
            winnerParticipationId,
            port.WinnerParticipationId!.Value.Value);

        Assert.Equal(5, port.Participant1Score);
        Assert.Equal(3, port.Participant2Score);
    }

    private sealed class FakeRecordMatchResultPort
        : IRecordMatchResultPort
    {
        public MatchId? MatchId { get; private set; }

        public ParticipationId? WinnerParticipationId { get; private set; }

        public int? Participant1Score { get; private set; }

        public int? Participant2Score { get; private set; }

        public Task RecordAsync(
            MatchId matchId,
            ParticipationId winnerParticipationId,
            int participant1Score,
            int participant2Score,
            CancellationToken cancellationToken)
        {
            MatchId = matchId;
            WinnerParticipationId = winnerParticipationId;
            Participant1Score = participant1Score;
            Participant2Score = participant2Score;

            return Task.CompletedTask;
        }
    }
}
