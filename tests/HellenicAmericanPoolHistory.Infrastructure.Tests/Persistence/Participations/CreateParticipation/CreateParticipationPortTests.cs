using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.CreateParticipation;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Participations.CreateParticipation;

public sealed class CreateParticipationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task CreateAsync_With_Valid_Participation_Should_Persist_Participation()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var participation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

        var port = new CreateParticipationPort(dbContext);

        var result = await port.CreateAsync(
            participation,
            CancellationToken.None);

        Assert.Equal(
            participation.Id,
            result);

        var persistedParticipation = await dbContext.Participations
            .SingleAsync(x => x.Id == participation.Id);

        Assert.Equal(
            participation.PlayerId,
            persistedParticipation.PlayerId);

        Assert.Equal(
            participation.TournamentId,
            persistedParticipation.TournamentId);

        Assert.Equal(
            participation.RegistrationDate,
            persistedParticipation.RegistrationDate);

        Assert.Equal(
            participation.Seed,
            persistedParticipation.Seed);
    }

    [Fact]
    public async Task CreateAsync_When_Player_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var participation = Participation.Create(
            PlayerId.New(),
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

        var port = new CreateParticipationPort(dbContext);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.CreateAsync(
                participation,
                CancellationToken.None));

        Assert.Equal(
            "Player not found.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_When_Tournament_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var participation = Participation.Create(
            data.Player.Id,
            TournamentId.New(),
            new DateOnly(2026, 8, 18),
            3);

        var port = new CreateParticipationPort(dbContext);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.CreateAsync(
                participation,
                CancellationToken.None));

        Assert.Equal(
            "Tournament not found.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_When_Tournament_Is_InProgress_Should_Throw_ConflictException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        data.Tournament.Schedule();
        data.Tournament.Start();

        await dbContext.SaveChangesAsync();

        var participation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

        var port = new CreateParticipationPort(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.CreateAsync(
                participation,
                CancellationToken.None));

        Assert.Equal(
            "Participation cannot be created because tournament status is 'InProgress'.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_When_Player_Is_Already_Registered_Should_Throw_ConflictException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var existingParticipation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Participations.Add(existingParticipation);

        await dbContext.SaveChangesAsync();

        var newParticipation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            2);

        var port = new CreateParticipationPort(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.CreateAsync(
                newParticipation,
                CancellationToken.None));

        Assert.Equal(
            "Player is already registered for this tournament.",
            exception.Message);
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
            $"Create Participation Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Create Participation Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Create Participation Test Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Create Participation Test",
            "Player One",
            new Country("Greece"));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            player);
    }

    private sealed record TestData(
        Tournament Tournament,
        Player Player);
}
