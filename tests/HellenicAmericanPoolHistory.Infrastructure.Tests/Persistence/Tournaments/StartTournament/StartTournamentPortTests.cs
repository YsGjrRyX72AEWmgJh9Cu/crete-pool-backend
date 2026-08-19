using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.StartTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.StartTournament;

public sealed class StartTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task StartAsync_Should_Start_Scheduled_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateScheduledTournamentAsync(dbContext);

        var port = new StartTournamentPort(dbContext);

        await port.StartAsync(
            tournament.Id,
            CancellationToken.None);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.InProgress,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task StartAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new StartTournamentPort(dbContext);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.StartAsync(
                TournamentId.New(),
                CancellationToken.None));
    }

    [Fact]
    public async Task StartAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_Scheduled()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new StartTournamentPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.StartAsync(
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
            "Start Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Start Tournament Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Start Tournament Test",
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

    private static async Task<Tournament> CreateScheduledTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
