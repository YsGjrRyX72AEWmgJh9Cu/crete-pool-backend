using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.DeleteVenue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Venues.DeleteVenue;

public sealed class DeleteVenuePortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task DeleteAsync_Should_Delete_Venue()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Delete Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var port = new DeleteVenuePort(dbContext);

        await port.DeleteAsync(
            venue.Id,
            CancellationToken.None);

        var persistedVenue =
            await dbContext.Venues
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    item => item.Id == venue.Id);

        Assert.Null(persistedVenue);
    }

    [Fact]
    public async Task DeleteAsync_When_Venue_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new DeleteVenuePort(dbContext);

        var exception =
            await Assert.ThrowsAsync<NotFoundException>(
                () => port.DeleteAsync(
                    new VenueId(Guid.NewGuid()),
                    CancellationToken.None));

        Assert.Equal(
            "Venue not found.",
            exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_When_Venue_Is_Used_By_Tournament_Should_Throw_ConflictException()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Delete Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        var tournament = Tournament.Create(
            new TournamentData(
                "Delete Venue Test Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo7,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                venue.Id,
                null));

        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var port = new DeleteVenuePort(dbContext);

        var exception =
            await Assert.ThrowsAsync<ConflictException>(
                () => port.DeleteAsync(
                    venue.Id,
                    CancellationToken.None));

        Assert.Equal(
            "Venue cannot be deleted because it is used by a tournament.",
            exception.Message);

        Assert.True(
            await dbContext.Venues
                .AnyAsync(
                    item => item.Id == venue.Id));
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
