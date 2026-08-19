using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.UpdateTournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.UpdateTournament;

public sealed class UpdateTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task UpdateAsync_Should_Update_Tournament()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            "Update Tournament Original Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Original Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Original Tournament Name",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var updatedData = new TournamentData(
            "Updated Tournament Name",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 8, 21),
            new DateOnly(2026, 8, 22),
            venue.Id);

        var port = new UpdateTournamentPort(dbContext);

        await port.UpdateAsync(
            tournament.Id,
            updatedData,
            CancellationToken.None);

        var updatedTournament = await dbContext.Tournaments
            .SingleAsync(
                item => item.Id == tournament.Id);

        Assert.Equal(
            "Updated Tournament Name",
            updatedTournament.Name);

        Assert.Equal(
            GameSet.RaceTo7,
            updatedTournament.GameSet);

        Assert.Equal(
            new DateOnly(2026, 8, 21),
            updatedTournament.StartDate);

        Assert.Equal(
            new DateOnly(2026, 8, 22),
            updatedTournament.EndDate);

        Assert.Equal(
            TournamentStatus.Draft,
            updatedTournament.TournamentStatus);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new UpdateTournamentPort(dbContext);

        var tournamentId = HellenicAmericanPoolHistory.Domain.Identifiers.TournamentId.New();

        var venue = Venue.Create(
            "Update Tournament Not Found Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Not Found Address"));

        var data = new TournamentData(
            "Updated Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 20),
            new DateOnly(2026, 8, 20),
            venue.Id);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.UpdateAsync(
                tournamentId,
                data,
                CancellationToken.None));

        Assert.Equal(
            "Tournament not found.",
            exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_InvalidOperationException_When_Tournament_Is_Not_Draft()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            "Update Tournament Status Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Status Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Scheduled Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 23),
                new DateOnly(2026, 8, 23),
                venue.Id));

        tournament.Schedule();

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var data = new TournamentData(
            "Should Not Update",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 8, 24),
            new DateOnly(2026, 8, 24),
            venue.Id);

        var port = new UpdateTournamentPort(dbContext);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => port.UpdateAsync(
                tournament.Id,
                data,
                CancellationToken.None));
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
