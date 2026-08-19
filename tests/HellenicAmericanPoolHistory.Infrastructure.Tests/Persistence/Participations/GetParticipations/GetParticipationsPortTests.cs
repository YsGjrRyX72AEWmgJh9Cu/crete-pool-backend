using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.GetParticipations;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Participations.GetParticipations;

public sealed class GetParticipationsPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Participations()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetParticipationsPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        Assert.Contains(
            result,
            participation => participation.Id == data.Participation1.Id.Value);

        Assert.Contains(
            result,
            participation => participation.Id == data.Participation2.Id.Value);

        var participation1 = result.Single(
            participation => participation.Id
                == data.Participation1.Id.Value);

        Assert.Equal(
            "Get Participations Test Player One",
            participation1.PlayerName);

        Assert.Equal(
            "Get Participations Test Tournament",
            participation1.TournamentName);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            participation1.RegistrationDate);

        Assert.Equal(
            1,
            participation1.Seed);

        Assert.Equal(
            data.Participation1.Status.ToString(),
            participation1.Status);

        var participation2 = result.Single(
            participation => participation.Id
                == data.Participation2.Id.Value);

        Assert.Equal(
            "Get Participations Test Player Two",
            participation2.PlayerName);

        Assert.Equal(
            "Get Participations Test Tournament",
            participation2.TournamentName);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            participation2.RegistrationDate);

        Assert.Equal(
            2,
            participation2.Seed);

        Assert.Equal(
            data.Participation2.Status.ToString(),
            participation2.Status);
    }

    [Fact]
    public async Task GetAllAsync_Should_Order_By_Tournament_Then_Player_Name()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateOrderingTestDataAsync(dbContext);

        var port = new GetParticipationsPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        var expectedIds = new[]
        {
            data.AlphaTournamentAlphaPlayer.Id.Value,
            data.AlphaTournamentBetaPlayer.Id.Value,
            data.BetaTournamentAlphaPlayer.Id.Value
        };

        var actualIds = result
            .Where(participation =>
                expectedIds.Contains(participation.Id))
            .Select(participation => participation.Id)
            .ToArray();

        Assert.Equal(
            expectedIds,
            actualIds);
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
            $"Get Participations Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Participations Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Participations Test Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player1 = Player.Create(
            "Get Participations Test",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Get Participations Test",
            "Player Two",
            new Country("Greece"));

        var participation1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        var participation2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            2);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(
            player1,
            player2);
        dbContext.Participations.AddRange(
            participation1,
            participation2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            player1,
            player2,
            participation1,
            participation2);
    }

    private static async Task<OrderingTestData> CreateOrderingTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Participations Ordering Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Participations Ordering Address {Guid.NewGuid():N}"));

        var alphaTournament = Tournament.Create(
            new TournamentData(
                $"A Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var betaTournament = Tournament.Create(
            new TournamentData(
                $"B Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var alphaPlayer = Player.Create(
            "A Player",
            "Alpha",
            new Country("Greece"));

        var betaPlayer = Player.Create(
            "B Player",
            "Beta",
            new Country("Greece"));

        var alphaTournamentAlphaPlayer = Participation.Create(
            alphaPlayer.Id,
            alphaTournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        var alphaTournamentBetaPlayer = Participation.Create(
            betaPlayer.Id,
            alphaTournament.Id,
            new DateOnly(2026, 8, 18),
            2);

        var betaTournamentAlphaPlayer = Participation.Create(
            alphaPlayer.Id,
            betaTournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.AddRange(
            alphaTournament,
            betaTournament);
        dbContext.Players.AddRange(
            alphaPlayer,
            betaPlayer);
        dbContext.Participations.AddRange(
            alphaTournamentAlphaPlayer,
            alphaTournamentBetaPlayer,
            betaTournamentAlphaPlayer);

        await dbContext.SaveChangesAsync();

        return new OrderingTestData(
            alphaTournamentAlphaPlayer,
            alphaTournamentBetaPlayer,
            betaTournamentAlphaPlayer);
    }

    private sealed record TestData(
        Tournament Tournament,
        Player Player1,
        Player Player2,
        Participation Participation1,
        Participation Participation2);

    private sealed record OrderingTestData(
        Participation AlphaTournamentAlphaPlayer,
        Participation AlphaTournamentBetaPlayer,
        Participation BetaTournamentAlphaPlayer);
}
