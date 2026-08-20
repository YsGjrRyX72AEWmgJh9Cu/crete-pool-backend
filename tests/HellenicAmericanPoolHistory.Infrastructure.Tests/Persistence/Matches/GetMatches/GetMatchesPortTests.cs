using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.GetMatches;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Matches.GetMatches;

public sealed class GetMatchesPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Matches()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetMatchesPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        Assert.Contains(
            response,
            match => match.Id == data.Match1.Id.Value);

        Assert.Contains(
            response,
            match => match.Id == data.Match2.Id.Value);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Match_Related_Data()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetMatchesPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        var match = response.Single(
            item => item.Id == data.Match1.Id.Value);

        Assert.Equal(
            data.Tournament1.Id.Value,
            match.TournamentId);

        Assert.Equal(
            "Get Matches Tournament Alpha",
            match.TournamentName);

        Assert.Equal(
            data.Participant1.Id.Value,
            match.Participant1Id);

        Assert.Equal(
            "Test Player One",
            match.Participant1PlayerName);

        Assert.Equal(
            data.Participant2.Id.Value,
            match.Participant2Id);

        Assert.Equal(
            "Test Player Two",
            match.Participant2PlayerName);

        Assert.Equal(
            data.Participant1.Id.Value,
            match.WinnerParticipationId);

        Assert.Equal(
            "Test Player One",
            match.WinnerPlayerName);

        Assert.Equal(5, match.Participant1Score);
        Assert.Equal(3, match.Participant2Score);
    }

    [Fact]
    public async Task GetAllAsync_Should_Order_By_Tournament_Name_Then_Start_Date()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetMatchesPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        var matches = response.ToList();

        var alphaIndex = matches.FindIndex(
            match => match.Id == data.Match1.Id.Value);

        var betaIndex = matches.FindIndex(
            match => match.Id == data.Match2.Id.Value);

        Assert.True(alphaIndex >= 0);
        Assert.True(betaIndex >= 0);
        Assert.True(alphaIndex < betaIndex);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Matches_Without_Result()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataWithoutResultAsync(dbContext);

        var port = new GetMatchesPort(dbContext);

        var response = await port.GetAllAsync(
            CancellationToken.None);

        var match = response.Single(
            item => item.Id == data.Match.Id.Value);

        Assert.Null(match.WinnerParticipationId);
        Assert.Null(match.WinnerPlayerName);
        Assert.Null(match.Participant1Score);
        Assert.Null(match.Participant2Score);
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
            "Get Matches Test Venue One",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Matches Address One"));

        var venue2 = Venue.Create(
            "Get Matches Test Venue Two",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Matches Address Two"));

        var tournament1 = Tournament.Create(
            new TournamentData(
                "Get Matches Tournament Alpha",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                venue1.Id));

        var tournament2 = Tournament.Create(
            new TournamentData(
                "Get Matches Tournament Beta",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 15),
                new DateOnly(2026, 8, 15),
                venue2.Id));

        var player1 = Player.Create(
            "Test",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Test",
            "Player Two",
            new Country("Greece"));

        var player3 = Player.Create(
            "Test",
            "Player Three",
            new Country("Greece"));

        var player4 = Player.Create(
            "Test",
            "Player Four",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament1.Id,
            new DateOnly(2026, 8, 14),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament1.Id,
            new DateOnly(2026, 8, 14),
            2);

        var participant3 = Participation.Create(
            player3.Id,
            tournament2.Id,
            new DateOnly(2026, 8, 15),
            1);

        var participant4 = Participation.Create(
            player4.Id,
            tournament2.Id,
            new DateOnly(2026, 8, 15),
            2);

        var match1 = new Match(
            MatchId.New(),
            tournament1.Id,
            participant1.Id,
            participant2.Id);

        match1.RecordResult(
            participant1.Id,
            5,
            3);

        var match2 = new Match(
            MatchId.New(),
            tournament2.Id,
            participant3.Id,
            participant4.Id);

        match2.RecordResult(
            participant3.Id,
            5,
            4);

        dbContext.Venues.AddRange(
            venue1,
            venue2);

        dbContext.Tournaments.AddRange(
            tournament1,
            tournament2);

        dbContext.Players.AddRange(
            player1,
            player2,
            player3,
            player4);

        dbContext.Participations.AddRange(
            participant1,
            participant2,
            participant3,
            participant4);

        dbContext.Matches.AddRange(
            match1,
            match2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament1,
            tournament2,
            participant1,
            participant2,
            participant3,
            participant4,
            match1,
            match2);
    }

    private static async Task<TestDataWithoutResult> CreateTestDataWithoutResultAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            "Get Matches No Result Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Get Matches No Result Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Matches No Result Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 16),
                new DateOnly(2026, 8, 16),
                venue.Id));

        var player1 = Player.Create(
            "Test",
            "Player Five",
            new Country("Greece"));

        var player2 = Player.Create(
            "Test",
            "Player Six",
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
        dbContext.Players.AddRange(
            player1,
            player2);
        dbContext.Participations.AddRange(
            participant1,
            participant2);
        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        return new TestDataWithoutResult(match);
    }

    private sealed record TestData(
        Tournament Tournament1,
        Tournament Tournament2,
        Participation Participant1,
        Participation Participant2,
        Participation Participant3,
        Participation Participant4,
        Match Match1,
        Match Match2);

    private sealed record TestDataWithoutResult(
        Match Match);
}
