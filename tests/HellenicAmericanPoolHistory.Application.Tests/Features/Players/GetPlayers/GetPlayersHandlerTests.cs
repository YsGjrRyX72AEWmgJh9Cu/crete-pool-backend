using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Players.GetPlayers;

public sealed class GetPlayersHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Players()
    {
        var expectedResponse = new GetPlayersResponse(
            Guid.NewGuid(),
            "Test",
            "Player",
            "Greece",
            new DateOnly(1990, 1, 1));

        var port = new FakeGetPlayersPort(
            new[] { expectedResponse });

        var handler = new GetPlayersHandler(port);

        var response = await handler.HandleAsync(
            new GetPlayersQuery(),
            CancellationToken.None);

        Assert.Single(response);
        Assert.Equal(
            expectedResponse,
            response[0]);
    }

    [Fact]
    public async Task HandleAsync_When_No_Players_Exist_Should_Return_Empty_List()
    {
        var port = new FakeGetPlayersPort(
            Array.Empty<GetPlayersResponse>());

        var handler = new GetPlayersHandler(port);

        var response = await handler.HandleAsync(
            new GetPlayersQuery(),
            CancellationToken.None);

        Assert.Empty(response);
    }

    private sealed class FakeGetPlayersPort
        : IGetPlayersPort
    {
        private readonly IReadOnlyList<GetPlayersResponse> _responses;

        public FakeGetPlayersPort(
            IReadOnlyList<GetPlayersResponse> responses)
        {
            _responses = responses;
        }

        public Task<IReadOnlyList<GetPlayersResponse>> GetAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_responses);
        }
    }
}
