using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.ScheduleTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.ScheduleTournament;

public sealed class ScheduleTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task ScheduleAsync_Should_Schedule_Draft_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new ScheduleTournamentPort(dbContext);

        await port.ScheduleAsync(
            tournament.Id,
            CancellationToken.None);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.Scheduled,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task ScheduleAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new ScheduleTournamentPort(dbContext);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.ScheduleAsync(
                TournamentId.New(),
                CancellationToken.None));
    }

    [Fact]
    public async Task ScheduleAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_Draft()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();

        await dbContext.SaveChangesAsync();

        var port = new ScheduleTournamentPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.ScheduleAsync(
                tournament.Id,
                CancellationToken.None));
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            "Schedule Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Schedule Tournament Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Schedule Tournament Test",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
