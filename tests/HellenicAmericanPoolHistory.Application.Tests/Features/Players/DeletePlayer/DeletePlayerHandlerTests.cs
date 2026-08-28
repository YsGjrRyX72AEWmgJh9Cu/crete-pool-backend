using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Players.DeletePlayer;

public sealed class DeletePlayerHandlerTests
{
    [Fact]
    public async Task Handle_Should_Pass_Command_To_Port()
    {
        var command = new DeletePlayerCommand(
            new PlayerId(Guid.NewGuid()));

        var port = new FakeDeletePlayerPort();

        var handler = new DeletePlayerHandler(port);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Same(
            command,
            port.DeletedCommand);
    }

    [Fact]
    public async Task Handle_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var port = new FakeDeletePlayerPort();

        var handler = new DeletePlayerHandler(port);

        var command = new DeletePlayerCommand(
            new PlayerId(Guid.NewGuid()));

        await handler.Handle(
            command,
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    private sealed class FakeDeletePlayerPort
        : IDeletePlayerPort
    {
        public DeletePlayerCommand? DeletedCommand { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task DeleteAsync(
            DeletePlayerCommand command,
            CancellationToken cancellationToken)
        {
            DeletedCommand = command;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
