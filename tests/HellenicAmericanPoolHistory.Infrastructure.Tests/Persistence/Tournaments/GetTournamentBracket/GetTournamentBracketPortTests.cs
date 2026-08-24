using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournamentBracket;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.GetTournamentBracket;

public sealed class GetTournamentBracketPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByTournamentIdAsync_Should_Return_Bracket_When_Tournament_Exists()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTournamentWithMatchesAsync(dbContext);

        var port = new GetTournamentBracketPort(dbContext);

        var response = await port.GetByTournamentIdAsync(
            data.Tournament.Id,
            CancellationToken.None);

        Assert.NotNull(response);

        Assert.Equal(
            data.Tournament.Id.Value,
            response.TournamentId);

        Assert.Equal(
            data.Tournament.Name,
            response.TournamentName);

        Assert.Equal(
            2,
            response.Rounds.Count);
    }

    [Fact]
    public async Task GetByTournamentIdAsync_Should_Group_Matches_By_Round_And_Order_By_Bracket_Position()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTournamentWithMatchesAsync(dbContext);

        var port = new GetTournamentBracketPort(dbContext);

        var response = await port.GetByTournamentIdAsync(
            data.Tournament.Id,
            CancellationToken.None);

        Assert.NotNull(response);

        var rounds = response.Rounds.ToList();

        Assert.Equal(
            1,
            rounds[0].Round);

        Assert.Equal(
            2,
            rounds[0].Matches.Count);

        Assert.Equal(
            1,
            rounds[0].Matches.First().BracketPosition);

        Assert.Equal(
            2,
            rounds[0].Matches.Last().BracketPosition);

        Assert.Equal(
            2,
            rounds[1].Round);

        Assert.Single(rounds[1].Matches);

        Assert.Equal(
            1,
            rounds[1].Matches.Single().BracketPosition);
    }

    [Fact]
    public async Task GetByTournamentIdAsync_Should_Return_Participant_Names_And_Result()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTournamentWithMatchesAsync(dbContext);

        var port = new GetTournamentBracketPort(dbContext);

        var response = await port.GetByTournamentIdAsync(
            data.Tournament.Id,
            CancellationToken.None);

        Assert.NotNull(response);

        var firstMatch = response.Rounds
            .Single(round => round.Round == 1)
            .Matches
            .Single(match => match.BracketPosition == 1);

        Assert.Equal(
            "Bracket Player 1 bracket-player-1@test.com",
            firstMatch.Participant1PlayerName);

        Assert.Equal(
            "Bracket Player 4 bracket-player-4@test.com",
            firstMatch.Participant2PlayerName);

        Assert.Equal(
            data.Participant1.Id.Value,
            firstMatch.WinnerParticipationId);

        Assert.Equal(
            "Bracket Player 1 bracket-player-1@test.com",
            firstMatch.WinnerPlayerName);

        Assert.Equal(
            5,
            firstMatch.Participant1Score);

        Assert.Equal(
            3,
            firstMatch.Participant2Score);
    }

    [Fact]
    public async Task GetByTournamentIdAsync_Should_Return_Null_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetTournamentBracketPort(dbContext);

        var response = await port.GetByTournamentIdAsync(
            TournamentId.New(),
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task GetByTournamentIdAsync_Should_Return_Empty_Rounds_When_Tournament_Has_No_Matches()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new GetTournamentBracketPort(dbContext);

        var response = await port.GetByTournamentIdAsync(
            tournament.Id,
            CancellationToken.None);

        Assert.NotNull(response);

        Assert.Empty(response.Rounds);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<TestData> CreateTournamentWithMatchesAsync(
        ApplicationDbContext dbContext)
    {
        var tournament = await CreateTournamentAsync(dbContext);

        var player1 = Player.Create(
            "Bracket Player 1",
            "bracket-player-1@test.com",
            new Country("Greece"));

        var player2 = Player.Create(
            "Bracket Player 2",
            "bracket-player-2@test.com",
            new Country("Greece"));

        var player3 = Player.Create(
            "Bracket Player 3",
            "bracket-player-3@test.com",
            new Country("Greece"));

        var player4 = Player.Create(
            "Bracket Player 4",
            "bracket-player-4@test.com",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 20),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 20),
            2);

        var participant3 = Participation.Create(
            player3.Id,
            tournament.Id,
            new DateOnly(2026, 8, 20),
            3);

        var participant4 = Participation.Create(
            player4.Id,
            tournament.Id,
            new DateOnly(2026, 8, 20),
            4);

        var match1 = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant4.Id);

        match1.RecordResult(
            participant1.Id,
            5,
            3);

        var match2 = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            2,
            participant2.Id,
            participant3.Id);

        var final = new Match(
            MatchId.New(),
            tournament.Id,
            2,
            1,
            participant1.Id,
            participant2.Id);

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
            match2,
            final);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Tournament Bracket Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Tournament Bracket Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Get Tournament Bracket Test {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1);
}
