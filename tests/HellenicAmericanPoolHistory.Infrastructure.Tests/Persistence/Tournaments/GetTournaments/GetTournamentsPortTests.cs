using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournaments;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.GetTournaments;

public sealed class GetTournamentsPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Tournaments()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetTournamentsPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        Assert.Contains(
            response,
            tournament => tournament.Id == data.Tournament1.Id.Value);

        Assert.Contains(
            response,
            tournament => tournament.Id == data.Tournament2.Id.Value);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Tournament_Data()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetTournamentsPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        var tournament = response.Single(
            item => item.Id == data.Tournament1.Id.Value);

        Assert.Equal(
            "Get Tournaments Tournament Alpha",
            tournament.Name);

        Assert.Equal(
            TournamentType.Individual,
            tournament.TournamentType);

        Assert.Equal(
            BracketType.SingleElimination,
            tournament.BracketType);

        Assert.Equal(
            GameSet.RaceTo5,
            tournament.GameSet);

        Assert.Equal(
            TournamentStatus.Draft,
            tournament.TournamentStatus);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            tournament.StartDate);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            tournament.EndDate);

        Assert.Equal(
            data.Tournament1.VenueId.Value,
            tournament.VenueId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Order_By_Start_Date_Then_Name()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetTournamentsPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        var tournaments = response.ToList();

        var alphaIndex = tournaments.FindIndex(
            tournament => tournament.Id == data.Tournament1.Id.Value);

        var betaIndex = tournaments.FindIndex(
            tournament => tournament.Id == data.Tournament2.Id.Value);

        Assert.True(alphaIndex >= 0);
        Assert.True(betaIndex >= 0);
        Assert.True(alphaIndex < betaIndex);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue1 = Venue.Create(
            "Get Tournaments Test Venue One",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Tournaments Address One"));

        var venue2 = Venue.Create(
            "Get Tournaments Test Venue Two",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Tournaments Address Two"));

        var tournament1 = Tournament.Create(
            new TournamentData(
                "Get Tournaments Tournament Alpha",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 18),
                new DateOnly(2026, 8, 18),
                venue1.Id));

        var tournament2 = Tournament.Create(
            new TournamentData(
                "Get Tournaments Tournament Beta",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 19),
                new DateOnly(2026, 8, 19),
                venue2.Id));

        dbContext.Venues.AddRange(
            venue1,
            venue2);

        dbContext.Tournaments.AddRange(
            tournament1,
            tournament2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament1,
            tournament2);
    }

    private sealed record TestData(
        Tournament Tournament1,
        Tournament Tournament2);
}
