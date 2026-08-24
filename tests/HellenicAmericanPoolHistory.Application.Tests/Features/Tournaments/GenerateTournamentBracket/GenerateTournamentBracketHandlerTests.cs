using HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.GenerateTournamentBracket;

public sealed class GenerateTournamentBracketHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_Port_With_Tournament_Id()
    {
        var tournamentId = TournamentId.New();

        var port = new FakeGenerateTournamentBracketPort();

        var handler = new GenerateTournamentBracketHandler(port);

        await handler.Handle(
            tournamentId.Value,
            CancellationToken.None);

        Assert.Equal(
            tournamentId,
            port.RequestedTournamentId);
    }

    [Fact]
    public async Task Handle_Should_Complete_When_Port_Succeeds()
    {
        var port = new FakeGenerateTournamentBracketPort();

        var handler = new GenerateTournamentBracketHandler(port);

        var exception = await Record.ExceptionAsync(
            () => handler.Handle(
                TournamentId.New().Value,
                CancellationToken.None));

        Assert.Null(exception);
        Assert.True(port.WasCalled);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Port_Exception()
    {
        var expectedException =
            new InvalidOperationException("Test exception.");

        var port = new FakeGenerateTournamentBracketPort(
            expectedException);

        var handler = new GenerateTournamentBracketHandler(port);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(
                TournamentId.New().Value,
                CancellationToken.None));

        Assert.Same(
            expectedException,
            exception);
    }

    private sealed class FakeGenerateTournamentBracketPort
        : IGenerateTournamentBracketPort
    {
        private readonly Exception? _exception;

        public FakeGenerateTournamentBracketPort(
            Exception? exception = null)
        {
            _exception = exception;
        }

        public TournamentId? RequestedTournamentId { get; private set; }

        public bool WasCalled { get; private set; }

        public Task GenerateAsync(
            TournamentId tournamentId,
            CancellationToken cancellationToken)
        {
            WasCalled = true;
            RequestedTournamentId = tournamentId;

            if (_exception is not null)
            {
                return Task.FromException(_exception);
            }

            return Task.CompletedTask;
        }
    }
}
