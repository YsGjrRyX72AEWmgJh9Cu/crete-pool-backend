using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.GetParticipation;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Participations.GetParticipation;

public sealed class GetParticipationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByIdAsync_With_Existing_Participation_Should_Return_Participation()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new GetParticipationPort(dbContext);

        var result = await port.GetByIdAsync(
            data.Participation.Id,
            CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(
            data.Participation.Id.Value,
            result!.Id);

        Assert.Equal(
            data.Player.Id.Value,
            result.PlayerId);

        Assert.Equal(
            "Get Participation Test Player One",
            result.PlayerName);

        Assert.Equal(
            data.Tournament.Id.Value,
            result.TournamentId);

        Assert.Equal(
            "Get Participation Test Tournament",
            result.TournamentName);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            result.RegistrationDate);

        Assert.Equal(
            3,
            result.Seed);

        Assert.Equal(
            data.Participation.Status.ToString(),
            result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_With_NonExisting_Participation_Should_Return_Null()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetParticipationPort(dbContext);

        var result = await port.GetByIdAsync(
            ParticipationId.New(),
            CancellationToken.None);

        Assert.Null(result);
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
            $"Get Participation Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Participation Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Participation Test Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Get Participation Test",
            "Player One",
            new Country("Greece"));

        var participation = Participation.Create(
            player.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.Add(player);
        dbContext.Participations.Add(participation);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            player,
            participation);
    }

    private sealed record TestData(
        Tournament Tournament,
        Player Player,
        Participation Participation);
}
