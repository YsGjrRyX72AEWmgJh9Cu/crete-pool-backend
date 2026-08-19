using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.GetTournament;

public sealed class GetTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByIdAsync_Should_Return_Tournament_When_It_Exists()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new GetTournamentPort(dbContext);

        var response = await port.GetByIdAsync(
            tournament.Id.Value,
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(
            tournament.Id.Value,
            response.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_All_Tournament_Data()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new GetTournamentPort(dbContext);

        var response = await port.GetByIdAsync(
            tournament.Id.Value,
            CancellationToken.None);

        Assert.NotNull(response);

        Assert.Equal(
            tournament.Id.Value,
            response.Id);

        Assert.Equal(
            "Get Tournament Test",
            response.Name);

        Assert.Equal(
            TournamentType.Individual,
            response.TournamentType);

        Assert.Equal(
            BracketType.SingleElimination,
            response.BracketType);

        Assert.Equal(
            GameSet.RaceTo5,
            response.GameSet);

        Assert.Equal(
            TournamentStatus.Draft,
            response.TournamentStatus);

        Assert.Equal(
            new DateOnly(2026, 8, 17),
            response.StartDate);

        Assert.Equal(
            new DateOnly(2026, 8, 17),
            response.EndDate);

        Assert.Equal(
            tournament.VenueId.Value,
            response.VenueId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetTournamentPort(dbContext);

        var response = await port.GetByIdAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.Null(response);
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
            "Get Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Tournament Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Tournament Test",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 17),
                new DateOnly(2026, 8, 17),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
