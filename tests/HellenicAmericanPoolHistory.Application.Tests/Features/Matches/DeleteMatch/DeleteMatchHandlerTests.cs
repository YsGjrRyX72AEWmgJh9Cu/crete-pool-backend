using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Matches.DeleteMatch;

public sealed class DeleteMatchHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_Port_With_Command()
    {
        var matchId = MatchId.New();

        var command = new DeleteMatchCommand(matchId);

        var port = new FakeDeleteMatchPort();
        var handler = new DeleteMatchHandler(port);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.NotNull(port.RequestedCommand);
        Assert.Equal(
            command,
            port.RequestedCommand);
    }

    [Fact]
    public async Task Handle_Should_Complete_When_Port_Succeeds()
    {
        var command = new DeleteMatchCommand(
            MatchId.New());

        var port = new FakeDeleteMatchPort();
        var handler = new DeleteMatchHandler(port);

        var exception = await Record.ExceptionAsync(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Null(exception);
        Assert.True(port.WasCalled);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Port_Exception()
    {
        var expectedException =
            new InvalidOperationException("Test exception.");

        var command = new DeleteMatchCommand(
            MatchId.New());

        var port = new FakeDeleteMatchPort(
            expectedException);

        var handler = new DeleteMatchHandler(port);

        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(
                    command,
                    CancellationToken.None));

        Assert.Same(
            expectedException,
            exception);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeDeleteMatchPort();
        var handler = new DeleteMatchHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.Handle(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeDeleteMatchPort
        : IDeleteMatchPort
    {
        private readonly Exception? _exception;

        public FakeDeleteMatchPort(
            Exception? exception = null)
        {
            _exception = exception;
        }

        public DeleteMatchCommand? RequestedCommand { get; private set; }

        public bool WasCalled { get; private set; }

        public Task DeleteAsync(
            DeleteMatchCommand command,
            CancellationToken cancellationToken)
        {
            WasCalled = true;
            RequestedCommand = command;

            if (_exception is not null)
            {
                return Task.FromException(_exception);
            }

            return Task.CompletedTask;
        }
    }
}
