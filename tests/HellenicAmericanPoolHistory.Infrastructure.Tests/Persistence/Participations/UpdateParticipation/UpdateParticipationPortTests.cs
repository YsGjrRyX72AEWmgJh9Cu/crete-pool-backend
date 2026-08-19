using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.UpdateParticipation;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Participations.UpdateParticipation;

public sealed class UpdateParticipationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task UpdateAsync_With_Valid_Changes_Should_Update_Participation()
    {
        await using var dbContext = CreateDbContext();

        var participation = await CreateParticipationAsync(dbContext);

        var port = new UpdateParticipationPort(dbContext);

        var command = new UpdateParticipationCommand(
            participation.Id,
            5,
            ParticipationStatus.CheckedIn);

        var result = await port.UpdateAsync(
            command,
            CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(
            participation.Id.Value,
            result!.Id);

        await dbContext.Entry(participation).ReloadAsync();

        Assert.Equal(
            5,
            participation.Seed);

        Assert.Equal(
            ParticipationStatus.CheckedIn,
            participation.Status);
    }

    [Fact]
    public async Task UpdateAsync_With_NonExisting_Participation_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new UpdateParticipationPort(dbContext);

        var command = new UpdateParticipationCommand(
            HellenicAmericanPoolHistory.Domain.Identifiers.ParticipationId.New(),
            5,
            ParticipationStatus.CheckedIn);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.UpdateAsync(
                command,
                CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_With_Invalid_Status_Transition_Should_Throw_ConflictException()
    {
        await using var dbContext = CreateDbContext();

        var participation = await CreateParticipationAsync(dbContext);

        var port = new UpdateParticipationPort(dbContext);

        var command = new UpdateParticipationCommand(
            participation.Id,
            5,
            ParticipationStatus.Completed);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.UpdateAsync(
                command,
                CancellationToken.None));

        Assert.Equal(
            "Participation status cannot change from 'Registered' to 'Completed'.",
            exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_With_Invalid_Seed_Should_Throw_ArgumentOutOfRangeException()
    {
        await using var dbContext = CreateDbContext();

        var participation = await CreateParticipationAsync(dbContext);

        var port = new UpdateParticipationPort(dbContext);

        var command = new UpdateParticipationCommand(
            participation.Id,
            0,
            ParticipationStatus.CheckedIn);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => port.UpdateAsync(
                command,
                CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_With_Null_Command_Should_Throw_ArgumentNullException()
    {
        await using var dbContext = CreateDbContext();

        var port = new UpdateParticipationPort(dbContext);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => port.UpdateAsync(
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

    private static async Task<Participation> CreateParticipationAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Update Participation Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Update Participation Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Update Participation Test Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Update Participation Test",
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

        return participation;
    }
}
