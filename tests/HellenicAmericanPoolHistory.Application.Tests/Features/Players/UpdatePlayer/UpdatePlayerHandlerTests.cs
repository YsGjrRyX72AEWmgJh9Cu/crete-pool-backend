using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Players.UpdatePlayer;

public sealed class UpdatePlayerHandlerTests
{
    [Fact]
    public async Task Handle_Should_Pass_Command_To_Port()
    {
        var playerId = new PlayerId(Guid.NewGuid());

        var command = new UpdatePlayerCommand(
            playerId,
            "Updated",
            "Player",
            "Greece",
            new DateOnly(1991, 2, 3));

        var port = new FakeUpdatePlayerPort();

        var handler = new UpdatePlayerHandler(port);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Same(
            command,
            port.UpdatedCommand);
    }

    [Fact]
    public async Task Handle_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var command = new UpdatePlayerCommand(
            new PlayerId(Guid.NewGuid()),
            "Updated",
            "Player",
            "Greece",
            null);

        var port = new FakeUpdatePlayerPort();

        var handler = new UpdatePlayerHandler(port);

        await handler.Handle(
            command,
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    private sealed class FakeUpdatePlayerPort
        : IUpdatePlayerPort
    {
        public UpdatePlayerCommand? UpdatedCommand { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task UpdateAsync(
            UpdatePlayerCommand command,
            CancellationToken cancellationToken)
        {
            UpdatedCommand = command;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
