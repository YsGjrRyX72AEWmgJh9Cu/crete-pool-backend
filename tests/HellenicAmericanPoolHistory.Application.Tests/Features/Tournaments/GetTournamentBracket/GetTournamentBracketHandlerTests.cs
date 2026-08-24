using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.GetTournamentBracket;

public sealed class GetTournamentBracketHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Query_Should_Return_Bracket()
    {
        var tournamentId = TournamentId.New();

        var expectedResponse =
            new GetTournamentBracketResponse(
                tournamentId.Value,
                "Test Tournament",
                new[]
                {
                    new GetTournamentBracketRoundResponse(
                        1,
                        new[]
                        {
                            new GetTournamentBracketMatchResponse(
                                Guid.NewGuid(),
                                1,
                                Guid.NewGuid(),
                                "Player One",
                                Guid.NewGuid(),
                                "Player Two",
                                null,
                                null,
                                null,
                                null)
                        })
                });

        var port = new FakeGetTournamentBracketPort(
            expectedResponse);

        var handler = new GetTournamentBracketHandler(port);

        var query = new GetTournamentBracketQuery(
            tournamentId.Value);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(
            expectedResponse,
            response);
        Assert.Equal(
            tournamentId.Value,
            response.TournamentId);
    }

    [Fact]
    public async Task HandleAsync_When_Tournament_Does_Not_Exist_Should_Return_Null()
    {
        var port = new FakeGetTournamentBracketPort(null);

        var handler = new GetTournamentBracketHandler(port);

        var query = new GetTournamentBracketQuery(
            TournamentId.New().Value);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_Tournament_Id_To_Port()
    {
        var tournamentId = TournamentId.New();

        var port = new FakeGetTournamentBracketPort(null);

        var handler = new GetTournamentBracketHandler(port);

        await handler.HandleAsync(
            new GetTournamentBracketQuery(
                tournamentId.Value),
            CancellationToken.None);

        Assert.Equal(
            tournamentId.Value,
            port.RequestedTournamentId);
    }

    private sealed class FakeGetTournamentBracketPort
        : IGetTournamentBracketPort
    {
        private readonly GetTournamentBracketResponse? _response;

        public FakeGetTournamentBracketPort(
            GetTournamentBracketResponse? response)
        {
            _response = response;
        }

        public Guid? RequestedTournamentId { get; private set; }

        public Task<GetTournamentBracketResponse?> GetByTournamentIdAsync(
            TournamentId tournamentId,
            CancellationToken cancellationToken)
        {
            RequestedTournamentId = tournamentId.Value;

            return Task.FromResult(_response);
        }
    }
}
