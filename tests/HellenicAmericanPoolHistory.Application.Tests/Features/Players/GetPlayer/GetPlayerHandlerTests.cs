using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Players.GetPlayer;

public sealed class GetPlayerHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Player_From_Port()
    {
        var playerId = Guid.NewGuid();

        var expectedResponse = new GetPlayerResponse(
            playerId,
            "Test",
            "Player",
            "Greece",
            new DateOnly(1990, 1, 1));

        var port = new FakeGetPlayerPort(
            expectedResponse);

        var handler = new GetPlayerHandler(port);

        var response = await handler.HandleAsync(
            new GetPlayerQuery(
                new PlayerId(playerId)),
            CancellationToken.None);

        Assert.Equal(
            expectedResponse,
            response);

        Assert.Equal(
            new PlayerId(playerId),
            port.PlayerId);
    }

    [Fact]
    public async Task HandleAsync_When_Player_Does_Not_Exist_Should_Return_Null()
    {
        var port = new FakeGetPlayerPort(
            null);

        var handler = new GetPlayerHandler(port);

        var response = await handler.HandleAsync(
            new GetPlayerQuery(
                new PlayerId(Guid.NewGuid())),
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Query_Is_Null()
    {
        var port = new FakeGetPlayerPort(
            null);

        var handler = new GetPlayerHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeGetPlayerPort
        : IGetPlayerPort
    {
        private readonly GetPlayerResponse? _response;

        public PlayerId PlayerId { get; private set; }

        public FakeGetPlayerPort(
            GetPlayerResponse? response)
        {
            _response = response;
        }

        public Task<GetPlayerResponse?> GetByIdAsync(
            PlayerId playerId,
            CancellationToken cancellationToken)
        {
            PlayerId = playerId;

            return Task.FromResult(_response);
        }
    }
}
