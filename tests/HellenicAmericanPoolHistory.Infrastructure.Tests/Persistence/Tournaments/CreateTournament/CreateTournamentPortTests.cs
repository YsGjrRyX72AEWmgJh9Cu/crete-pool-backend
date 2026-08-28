using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CreateTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.CreateTournament;

public sealed class CreateTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task SaveAsync_Should_Persist_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Create Tournament Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var tournament = Tournament.Create(
            new TournamentData(
                $"Create Tournament Test {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo7,
                new DateOnly(2026, 9, 1),
                new DateOnly(2026, 9, 3),
                venue.Id,
                null));

        var port = new CreateTournamentPort(dbContext);

        await port.SaveAsync(
            tournament,
            CancellationToken.None);

        var persistedTournament =
            await dbContext.Tournaments
                .AsNoTracking()
                .SingleAsync(
                    item => item.Id == tournament.Id);

        Assert.Equal(
            tournament.Id,
            persistedTournament.Id);

        Assert.Equal(
            tournament.Name,
            persistedTournament.Name);

        Assert.Equal(
            tournament.TournamentType,
            persistedTournament.TournamentType);

        Assert.Equal(
            tournament.BracketType,
            persistedTournament.BracketType);

        Assert.Equal(
            tournament.GameSet,
            persistedTournament.GameSet);

        Assert.Equal(
            tournament.StartDate,
            persistedTournament.StartDate);

        Assert.Equal(
            tournament.EndDate,
            persistedTournament.EndDate);

        Assert.Equal(
            tournament.VenueId,
            persistedTournament.VenueId);

        Assert.Equal(
            tournament.TournamentStatus,
            persistedTournament.TournamentStatus);
    }

    [Fact]
    public async Task SaveAsync_With_Null_Tournament_Should_Throw_ArgumentNullException()
    {
        await using var dbContext = CreateDbContext();

        var port = new CreateTournamentPort(dbContext);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => port.SaveAsync(
                null!,
                CancellationToken.None));
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
