using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CompleteTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.CompleteTournament;

public sealed class CompleteTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task CompleteAsync_Should_Complete_InProgress_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateInProgressTournamentAsync(dbContext);

        var port = new CompleteTournamentPort(dbContext);

        await port.CompleteAsync(
            tournament.Id,
            CancellationToken.None);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.Completed,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task CompleteAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new CompleteTournamentPort(dbContext);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.CompleteAsync(
                TournamentId.New(),
                CancellationToken.None));
    }

    [Fact]
    public async Task CompleteAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_InProgress()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new CompleteTournamentPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.CompleteAsync(
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
            "Complete Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Complete Tournament Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Complete Tournament Test",
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

    private static async Task<Tournament> CreateInProgressTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();
        tournament.Start();

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
