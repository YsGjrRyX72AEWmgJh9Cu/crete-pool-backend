using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CancelTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.CancelTournament;

public sealed class CancelTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task CancelAsync_Should_Cancel_Draft_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(
            dbContext,
            "Cancel Draft Tournament");

        var port = new CancelTournamentPort(dbContext);

        await port.CancelAsync(
            tournament.Id,
            CancellationToken.None);

        Assert.Equal(
            TournamentStatus.Cancelled,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task CancelAsync_Should_Cancel_Scheduled_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(
            dbContext,
            "Cancel Scheduled Tournament");

        tournament.Schedule();

        await dbContext.SaveChangesAsync();

        var port = new CancelTournamentPort(dbContext);

        await port.CancelAsync(
            tournament.Id,
            CancellationToken.None);

        Assert.Equal(
            TournamentStatus.Cancelled,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task CancelAsync_Should_Throw_ConflictException_When_Tournament_Is_InProgress()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(
            dbContext,
            "Cancel InProgress Tournament");

        tournament.Schedule();
        tournament.Start();

        await dbContext.SaveChangesAsync();

        var port = new CancelTournamentPort(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.CancelAsync(
                tournament.Id,
                CancellationToken.None));

        Assert.Contains(
            "Only tournaments in Draft or Scheduled status can be cancelled.",
            exception.Message);
    }

    [Fact]
    public async Task CancelAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new CancelTournamentPort(dbContext);

        var tournamentId =
            HellenicAmericanPoolHistory.Domain.Identifiers.TournamentId.New();

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.CancelAsync(
                tournamentId,
                CancellationToken.None));

        Assert.Equal(
            "Tournament not found.",
            exception.Message);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext,
        string name)
    {
        var venue = Venue.Create(
            $"{name} Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"{name} Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                name,
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 27),
                new DateOnly(2026, 8, 27),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
