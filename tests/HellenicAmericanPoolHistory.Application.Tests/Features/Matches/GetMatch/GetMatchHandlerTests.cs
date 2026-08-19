using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Matches.GetMatch;

public sealed class GetMatchHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Query_Should_Return_Match()
    {
        var matchId = MatchId.New();

        var expectedResponse = new GetMatchResponse(
            matchId.Value,
            Guid.NewGuid(),
            "Test Tournament",
            Guid.NewGuid(),
            "Player One",
            Guid.NewGuid(),
            "Player Two",
            Guid.NewGuid(),
            "Player One",
            5,
            3);

        var port = new FakeGetMatchPort(expectedResponse);
        var handler = new GetMatchHandler(port);

        var query = new GetMatchQuery(matchId);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(expectedResponse, response);
        Assert.Equal(matchId.Value, response.Id);
    }

    [Fact]
    public async Task HandleAsync_When_Match_Does_Not_Exist_Should_Return_Null()
    {
        var port = new FakeGetMatchPort(null);
        var handler = new GetMatchHandler(port);

        var query = new GetMatchQuery(MatchId.New());

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_Match_Id_To_Port()
    {
        var matchId = MatchId.New();

        var port = new FakeGetMatchPort(null);
        var handler = new GetMatchHandler(port);

        await handler.HandleAsync(
            new GetMatchQuery(matchId),
            CancellationToken.None);

        Assert.Equal(matchId, port.RequestedMatchId);
    }

    private sealed class FakeGetMatchPort : IGetMatchPort
    {
        private readonly GetMatchResponse? _response;

        public FakeGetMatchPort(GetMatchResponse? response)
        {
            _response = response;
        }

        public MatchId? RequestedMatchId { get; private set; }

        public Task<GetMatchResponse?> GetByIdAsync(
            MatchId matchId,
            CancellationToken cancellationToken)
        {
            RequestedMatchId = matchId;

            return Task.FromResult(_response);
        }
    }
}
