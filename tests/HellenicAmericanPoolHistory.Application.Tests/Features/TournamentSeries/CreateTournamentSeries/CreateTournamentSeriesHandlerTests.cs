using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.TournamentSeries.CreateTournamentSeries;

public sealed class CreateTournamentSeriesHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Create_Tournament_Series_And_Return_Id()
    {
        var organizationId = Guid.NewGuid();

        var port = new FakeCreateTournamentSeriesPort();

        var handler = new CreateTournamentSeriesHandler(port);

        var command = new CreateTournamentSeriesCommand(
            "Test Tournament Series",
            organizationId);

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotEqual(
            Guid.Empty,
            response.TournamentSeriesId);

        Assert.NotNull(
            port.SavedTournamentSeries);

        Assert.Equal(
            command.Name,
            port.SavedTournamentSeries.Name);

        Assert.Equal(
            command.OrganizationId,
            port.SavedTournamentSeries.OrganizationId.Value);

        Assert.Equal(
            response.TournamentSeriesId,
            port.SavedTournamentSeries.Id.Value);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var port = new FakeCreateTournamentSeriesPort();

        var handler = new CreateTournamentSeriesHandler(port);

        await handler.HandleAsync(
            new CreateTournamentSeriesCommand(
                "Test Tournament Series",
                Guid.NewGuid()),
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeCreateTournamentSeriesPort();

        var handler = new CreateTournamentSeriesHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeCreateTournamentSeriesPort
        : ICreateTournamentSeriesPort
    {
        public TournamentSeriesEntity? SavedTournamentSeries { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task SaveAsync(
            TournamentSeriesEntity tournamentSeries,
            CancellationToken cancellationToken)
        {
            SavedTournamentSeries = tournamentSeries;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
