using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.DeleteTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.DeleteTournament;

public sealed class DeleteTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task DeleteAsync_Should_Delete_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var venue = CreateVenue();

        var tournament = Tournament.Create(
            new TournamentData(
                "Delete Tournament Test",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 25),
                new DateOnly(2026, 8, 25),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var port = new DeleteTournamentPort(dbContext);

        await port.DeleteAsync(
            tournament.Id,
            CancellationToken.None);

        var exists = await dbContext.Tournaments
            .AnyAsync(item => item.Id == tournament.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new DeleteTournamentPort(dbContext);

        var tournamentId =
            HellenicAmericanPoolHistory.Domain.Identifiers.TournamentId.New();

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.DeleteAsync(
                tournamentId,
                CancellationToken.None));

        Assert.Equal(
            "Tournament not found.",
            exception.Message);
    }

[Fact]
public async Task DeleteAsync_Should_Throw_ConflictException_When_Tournament_Has_Participations()
{
    Guid tournamentId;

    await using (var dbContext = CreateDbContext())
    {
        var venue = CreateVenue();

        var tournament = Tournament.Create(
            new TournamentData(
                "Delete Tournament With Participation",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 26),
                new DateOnly(2026, 8, 26),
                venue.Id));

        var player = Player.Create(
            "Delete",
            "Tournament Player",
            new Country("Greece"));

        var participation = Participation.Create(
            player.Id,
            tournament.Id,
            new DateOnly(2026, 8, 26),
            1);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.Add(player);
        dbContext.Participations.Add(participation);

        await dbContext.SaveChangesAsync();

        tournamentId = tournament.Id.Value;
    }

    await using (var dbContext = CreateDbContext())
    {
        var port = new DeleteTournamentPort(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.DeleteAsync(
                new HellenicAmericanPoolHistory.Domain.Identifiers.TournamentId(
                    tournamentId),
                CancellationToken.None));

        Assert.Equal(
            "Tournament cannot be deleted because it has participations.",
            exception.Message);
    }

    await using (var verificationContext = CreateDbContext())
    {
        var tournamentExists = await verificationContext.Tournaments
            .AnyAsync(item => item.Id == new HellenicAmericanPoolHistory.Domain.Identifiers.TournamentId(tournamentId));

        Assert.True(tournamentExists);    
    }
}

    private static Venue CreateVenue()
    {
        return Venue.Create(
            "Delete Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Delete Tournament Test Address"));
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
