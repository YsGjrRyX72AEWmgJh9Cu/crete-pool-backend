using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Players.CreatePlayer;

public sealed class CreatePlayerHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Create_Player_And_Return_Id()
    {
        var expectedId = new PlayerId(Guid.NewGuid());

        var port = new FakeCreatePlayerPort(expectedId);

        var handler = new CreatePlayerHandler(port);

        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            "Greece",
            new DateOnly(1990, 1, 1));

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.Equal(
            expectedId.Value,
            response.Id);

        Assert.NotNull(port.CreatedPlayer);
        Assert.Equal(
            "John",
            port.CreatedPlayer.FirstName);
        Assert.Equal(
            "Doe",
            port.CreatedPlayer.LastName);
        Assert.Equal(
            "Greece",
            port.CreatedPlayer.CountryOfOrigin.Value);
        Assert.Equal(
            new DateOnly(1990, 1, 1),
            port.CreatedPlayer.BirthDate);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_Cancellation_Token_To_Port()
    {
        var expectedId = new PlayerId(Guid.NewGuid());

        var port = new FakeCreatePlayerPort(expectedId);

        var handler = new CreatePlayerHandler(port);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            "Greece",
            null);

        await handler.HandleAsync(
            command,
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_When_Command_Is_Null_Should_Throw()
    {
        var port = new FakeCreatePlayerPort(
            new PlayerId(Guid.NewGuid()));

        var handler = new CreatePlayerHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeCreatePlayerPort
        : ICreatePlayerPort
    {
        private readonly PlayerId _playerId;

        public FakeCreatePlayerPort(PlayerId playerId)
        {
            _playerId = playerId;
        }

        public Player? CreatedPlayer { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task<PlayerId> CreateAsync(
            Player player,
            CancellationToken cancellationToken = default)
        {
            CreatedPlayer = player;
            CancellationToken = cancellationToken;

            return Task.FromResult(_playerId);
        }
    }
}
