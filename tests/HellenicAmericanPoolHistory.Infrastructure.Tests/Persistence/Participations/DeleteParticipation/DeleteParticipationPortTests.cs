using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.DeleteParticipation;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Participations.DeleteParticipation;

public sealed class DeleteParticipationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task DeleteAsync_With_Existing_Participation_Should_Delete_Participation()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var participation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Participations.Add(participation);

        await dbContext.SaveChangesAsync();

        var port = new DeleteParticipationPort(dbContext);

        var command = new DeleteParticipationCommand(
            participation.Id);

        await port.DeleteAsync(
            command,
            CancellationToken.None);

        var deletedParticipation = await dbContext.Participations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == participation.Id);

        Assert.Null(deletedParticipation);
    }

    [Fact]
    public async Task DeleteAsync_With_NonExisting_Participation_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new DeleteParticipationPort(dbContext);

        var command = new DeleteParticipationCommand(
            HellenicAmericanPoolHistory.Domain.Identifiers.ParticipationId.New());

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.DeleteAsync(
                command,
                CancellationToken.None));
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
            $"Delete Participation Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Delete Participation Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Delete Participation Test Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Delete Participation Test",
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
