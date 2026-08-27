using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.TournamentSeries.GetTournamentSeries;

public sealed class GetTournamentSeriesHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_TournamentSeries_From_Port()
    {
        var tournamentSeries =
            new List<GetTournamentSeriesResponse>
            {
                new(
                    Guid.NewGuid(),
                    "Series A",
                    Guid.NewGuid()),

                new(
                    Guid.NewGuid(),
                    "Series B",
                    Guid.NewGuid())
            };

        var port =
            new FakeGetTournamentSeriesPort(
                tournamentSeries);

        var handler =
            new GetTournamentSeriesHandler(port);

        var result =
            await handler.HandleAsync(
                CancellationToken.None);

        Assert.Equal(
            tournamentSeries,
            result);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Empty_List_When_Port_Returns_Empty_List()
    {
        var tournamentSeries =
            Array.Empty<GetTournamentSeriesResponse>();

        var port =
            new FakeGetTournamentSeriesPort(
                tournamentSeries);

        var handler =
            new GetTournamentSeriesHandler(port);

        var result =
            await handler.HandleAsync(
                CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Port()
    {
        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var port =
            new FakeGetTournamentSeriesPort(
                Array.Empty<GetTournamentSeriesResponse>());

        var handler =
            new GetTournamentSeriesHandler(port);

        await handler.HandleAsync(
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.ReceivedCancellationToken);
    }

    private sealed class FakeGetTournamentSeriesPort(
        IReadOnlyList<GetTournamentSeriesResponse> tournamentSeries)
        : IGetTournamentSeriesPort
    {
        public CancellationToken ReceivedCancellationToken { get; private set; }

        public Task<IReadOnlyList<GetTournamentSeriesResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            ReceivedCancellationToken =
                cancellationToken;

            return Task.FromResult(
                tournamentSeries);
        }
    }
}
