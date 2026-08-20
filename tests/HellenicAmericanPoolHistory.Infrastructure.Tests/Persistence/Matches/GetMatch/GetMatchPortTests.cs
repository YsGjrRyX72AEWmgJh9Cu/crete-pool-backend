using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.GetMatch;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Matches.GetMatch;

public sealed class GetMatchPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByIdAsync_Should_Return_Match()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetMatchPort(dbContext);

        var response = await port.GetByIdAsync(
            data.Match.Id,
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(data.Match.Id.Value, response.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Match_Related_Data()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetMatchPort(dbContext);

        var response = await port.GetByIdAsync(
            data.Match.Id,
            CancellationToken.None);

        Assert.NotNull(response);

        Assert.Equal(
            data.Tournament.Id.Value,
            response.TournamentId);

        Assert.Equal(
            "Get Match Test Tournament",
            response.TournamentName);

        Assert.Equal(
            data.Participant1.Id.Value,
            response.Participant1Id);

        Assert.Equal(
            "Test Player One",
            response.Participant1PlayerName);

        Assert.Equal(
            data.Participant2.Id.Value,
            response.Participant2Id);

        Assert.Equal(
            "Test Player Two",
            response.Participant2PlayerName);

        Assert.Equal(
            data.Participant1.Id.Value,
            response.WinnerParticipationId);

        Assert.Equal(
            "Test Player One",
            response.WinnerPlayerName);

        Assert.Equal(5, response.Participant1Score);
        Assert.Equal(3, response.Participant2Score);
    }

    [Fact]
    public async Task GetByIdAsync_When_Match_Does_Not_Exist_Should_Return_Null()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetMatchPort(dbContext);

        var response = await port.GetByIdAsync(
            MatchId.New(),
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task GetByIdAsync_When_Match_Has_No_Result_Should_Return_Null_Result_Fields()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataWithoutResultAsync(dbContext);

        var port = new GetMatchPort(dbContext);

        var response = await port.GetByIdAsync(
            data.Match.Id,
            CancellationToken.None);

        Assert.NotNull(response);

        Assert.Null(response.WinnerParticipationId);
        Assert.Null(response.WinnerPlayerName);
        Assert.Null(response.Participant1Score);
        Assert.Null(response.Participant2Score);
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
        var venue = Venue.Create(
            "Get Match Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Match Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Match Test Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                venue.Id));

        var player1 = Player.Create(
            "Test",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Test",
            "Player Two",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            2);

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            participant1.Id,
            participant2.Id);

        match.RecordResult(
            participant1.Id,
            5,
            3);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(player1, player2);
        dbContext.Participations.AddRange(
            participant1,
            participant2);
        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2,
            match);
    }

    private static async Task<TestData> CreateTestDataWithoutResultAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            "Get Match No Result Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Match No Result Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Match No Result Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 16),
                new DateOnly(2026, 8, 16),
                venue.Id));

        var player1 = Player.Create(
            "Test",
            "Player Three",
            new Country("Greece"));

        var player2 = Player.Create(
            "Test",
            "Player Four",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 16),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 16),
            2);

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            participant1.Id,
            participant2.Id);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(player1, player2);
        dbContext.Participations.AddRange(
            participant1,
            participant2);
        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2,
            match);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2,
        Match Match);
}
