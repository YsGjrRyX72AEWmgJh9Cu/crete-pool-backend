using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.AdvanceTournamentBracket;

public sealed class AdvanceTournamentBracketHandlerTests
{
    [Fact]
    public async Task Handle_Should_Advance_Tournament_Bracket()
    {
        var tournamentId = TournamentId.New();

        var port = new FakeAdvanceTournamentBracketPort();

        var handler = new AdvanceTournamentBracketHandler(port);

        await handler.Handle(
            tournamentId.Value,
            CancellationToken.None);

        Assert.Equal(
            tournamentId.Value,
            port.RequestedTournamentId);
    }

    [Fact]
    public async Task Handle_Should_Pass_Cancellation_Token_To_Port()
    {
        var tournamentId = TournamentId.New();

        var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var port = new FakeAdvanceTournamentBracketPort();

        var handler = new AdvanceTournamentBracketHandler(port);

        await handler.Handle(
            tournamentId.Value,
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.RequestedCancellationToken);
    }

    private sealed class FakeAdvanceTournamentBracketPort
        : IAdvanceTournamentBracketPort
    {
        public Guid? RequestedTournamentId { get; private set; }

        public CancellationToken RequestedCancellationToken
        {
            get;
            private set;
        }

        public Task AdvanceAsync(
            TournamentId tournamentId,
            CancellationToken cancellationToken)
        {
            RequestedTournamentId = tournamentId.Value;
            RequestedCancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
