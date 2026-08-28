using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.CreateTournament;

public sealed class CreateTournamentHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Create_Tournament_And_Save_It()
    {
        var venueId = Guid.NewGuid();
        var seriesId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 9, 1);
        var endDate = new DateOnly(2026, 9, 3);

        var command = new CreateTournamentCommand(
            "Test Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            startDate,
            endDate,
            venueId,
            seriesId);

        var port = new FakeCreateTournamentPort();

        var handler = new CreateTournamentHandler(port);

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotEqual(
            Guid.Empty,
            response.TournamentId);

        Assert.NotNull(port.Tournament);

        Assert.Equal(
            response.TournamentId,
            port.Tournament.Id.Value);

        Assert.Equal(
            command.Name,
            port.Tournament.Name);

        Assert.Equal(
            command.TournamentType,
            port.Tournament.TournamentType);

        Assert.Equal(
            command.BracketType,
            port.Tournament.BracketType);

        Assert.Equal(
            command.GameSet,
            port.Tournament.GameSet);

        Assert.Equal(
            command.StartDate,
            port.Tournament.StartDate);

        Assert.Equal(
            command.EndDate,
            port.Tournament.EndDate);

        Assert.Equal(
            venueId,
            port.Tournament.VenueId.Value);

        Assert.NotNull(
            port.Tournament.TournamentSeriesId);

        Assert.Equal(
            seriesId,
            port.Tournament.TournamentSeriesId!.Value.Value);

        Assert.Equal(
            TournamentStatus.Draft,
            port.Tournament.TournamentStatus);
    }

    [Fact]
    public async Task HandleAsync_Should_Create_Tournament_Without_Series()
    {
        var command = new CreateTournamentCommand(
            "Test Tournament Without Series",
            TournamentType.Team,
            BracketType.SingleElimination,
            GameSet.RaceTo9,
            new DateOnly(2026, 10, 1),
            new DateOnly(2026, 10, 2),
            Guid.NewGuid(),
            null);

        var port = new FakeCreateTournamentPort();

        var handler = new CreateTournamentHandler(port);

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotEqual(
            Guid.Empty,
            response.TournamentId);

        Assert.NotNull(port.Tournament);

        Assert.Null(
            port.Tournament.TournamentSeriesId);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeCreateTournamentPort();

        var handler = new CreateTournamentHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeCreateTournamentPort
        : ICreateTournamentPort
    {
        public Tournament? Tournament { get; private set; }

        public Task SaveAsync(
            Tournament tournament,
            CancellationToken cancellationToken)
        {
            Tournament = tournament;

            return Task.CompletedTask;
        }
    }
}
