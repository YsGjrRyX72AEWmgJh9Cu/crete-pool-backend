using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.GetTournaments;

public sealed class GetTournamentsHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Tournaments()
    {
        var expectedResponse = new List<GetTournamentsResponse>
        {
            new(
                Guid.NewGuid(),
                "Tournament One",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                TournamentStatus.Draft,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                Guid.NewGuid()),
            new(
                Guid.NewGuid(),
                "Tournament Two",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo7,
                TournamentStatus.Scheduled,
                new DateOnly(2026, 8, 15),
                new DateOnly(2026, 8, 15),
                Guid.NewGuid())
        };

        var port = new FakeGetTournamentsPort(
            expectedResponse);

        var handler = new GetTournamentsHandler(port);

        var response = await handler.HandleAsync(
            CancellationToken.None);

        Assert.Equal(
            expectedResponse,
            response);
    }

    [Fact]
    public async Task HandleAsync_When_No_Tournaments_Exist_Should_Return_Empty_List()
    {
        var port = new FakeGetTournamentsPort(
            Array.Empty<GetTournamentsResponse>());

        var handler = new GetTournamentsHandler(port);

        var response = await handler.HandleAsync(
            CancellationToken.None);

        Assert.Empty(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Port_Response()
    {
        var expectedResponse = new List<GetTournamentsResponse>
        {
            new(
                Guid.NewGuid(),
                "Test Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                TournamentStatus.Draft,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                Guid.NewGuid())
        };

        var port = new FakeGetTournamentsPort(
            expectedResponse);

        var handler = new GetTournamentsHandler(port);

        var response = await handler.HandleAsync(
            CancellationToken.None);

        Assert.Same(
            expectedResponse,
            response);
    }

    private sealed class FakeGetTournamentsPort
        : IGetTournamentsPort
    {
        private readonly IReadOnlyList<GetTournamentsResponse> _response;

        public FakeGetTournamentsPort(
            IReadOnlyList<GetTournamentsResponse> response)
        {
            _response = response;
        }

        public Task<IReadOnlyList<GetTournamentsResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
