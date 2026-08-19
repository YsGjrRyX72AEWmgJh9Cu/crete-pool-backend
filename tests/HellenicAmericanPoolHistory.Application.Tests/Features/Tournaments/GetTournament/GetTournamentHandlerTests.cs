using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.GetTournament;

public sealed class GetTournamentHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Query_Should_Return_Tournament()
    {
        var tournamentId = TournamentId.New();

        var expectedResponse = new GetTournamentResponse(
            tournamentId.Value,
            "Test Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            TournamentStatus.Draft,
            new DateOnly(2026, 8, 14),
            new DateOnly(2026, 8, 14),
            Guid.NewGuid());

        var port = new FakeGetTournamentPort(
            expectedResponse);

        var handler = new GetTournamentHandler(port);

        var query = new GetTournamentQuery(
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
            response.Id);
    }

    [Fact]
    public async Task HandleAsync_When_Tournament_Does_Not_Exist_Should_Return_Null()
    {
        var port = new FakeGetTournamentPort(null);

        var handler = new GetTournamentHandler(port);

        var query = new GetTournamentQuery(
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

        var port = new FakeGetTournamentPort(null);

        var handler = new GetTournamentHandler(port);

        await handler.HandleAsync(
            new GetTournamentQuery(
                tournamentId.Value),
            CancellationToken.None);

        Assert.Equal(
            tournamentId.Value,
            port.RequestedTournamentId);
    }

    private sealed class FakeGetTournamentPort
        : IGetTournamentPort
    {
        private readonly GetTournamentResponse? _response;

        public FakeGetTournamentPort(
            GetTournamentResponse? response)
        {
            _response = response;
        }

        public Guid? RequestedTournamentId { get; private set; }

        public Task<GetTournamentResponse?> GetByIdAsync(
            Guid tournamentId,
            CancellationToken cancellationToken)
        {
            RequestedTournamentId = tournamentId;

            return Task.FromResult(_response);
        }
    }
}
