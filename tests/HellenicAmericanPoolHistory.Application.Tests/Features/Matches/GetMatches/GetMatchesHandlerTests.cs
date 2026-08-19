using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Matches.GetMatches;

public sealed class GetMatchesHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Matches_From_Port()
    {
        var expectedResponses = new[]
        {
            new GetMatchesResponse(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Tournament One",
                Guid.NewGuid(),
                "Player One",
                Guid.NewGuid(),
                "Player Two",
                Guid.NewGuid(),
                "Player One",
                5,
                3),
            new GetMatchesResponse(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Tournament Two",
                Guid.NewGuid(),
                "Player Three",
                Guid.NewGuid(),
                "Player Four",
                Guid.NewGuid(),
                "Player Four",
                5,
                4)
        };

        var port = new FakeGetMatchesPort(expectedResponses);
        var handler = new GetMatchesHandler(port);

        var response = await handler.HandleAsync(
            new GetMatchesQuery(),
            CancellationToken.None);

        Assert.Equal(expectedResponses, response);
    }

    [Fact]
    public async Task HandleAsync_When_No_Matches_Exist_Should_Return_Empty_Collection()
    {
        var port = new FakeGetMatchesPort([]);
        var handler = new GetMatchesHandler(port);

        var response = await handler.HandleAsync(
            new GetMatchesQuery(),
            CancellationToken.None);

        Assert.Empty(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Call_Port()
    {
        var port = new FakeGetMatchesPort([]);
        var handler = new GetMatchesHandler(port);

        await handler.HandleAsync(
            new GetMatchesQuery(),
            CancellationToken.None);

        Assert.True(port.WasCalled);
    }

    private sealed class FakeGetMatchesPort : IGetMatchesPort
    {
        private readonly IReadOnlyCollection<GetMatchesResponse> _responses;

        public FakeGetMatchesPort(
            IReadOnlyCollection<GetMatchesResponse> responses)
        {
            _responses = responses;
        }

        public bool WasCalled { get; private set; }

        public Task<IReadOnlyCollection<GetMatchesResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            WasCalled = true;

            return Task.FromResult(_responses);
        }
    }
}
