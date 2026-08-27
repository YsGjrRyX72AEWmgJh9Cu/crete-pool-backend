using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.CreateVenue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Venues.CreateVenue;

public sealed class CreateVenuePortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task SaveAsync_Should_Persist_Venue()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Infrastructure Create Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        var port = new CreateVenuePort(dbContext);

        await port.SaveAsync(
            venue,
            CancellationToken.None);

        var persistedVenue =
            await dbContext.Venues
                .SingleAsync(
                    item => item.Id == venue.Id);

        Assert.Equal(
            venue.Id,
            persistedVenue.Id);

        Assert.Equal(
            venue.Name,
            persistedVenue.Name);

        Assert.Equal(
            venue.Location.Country,
            persistedVenue.Location.Country);

        Assert.Equal(
            venue.Location.City,
            persistedVenue.Location.City);

        Assert.Equal(
            venue.Location.Address,
            persistedVenue.Location.Address);
    }

    [Fact]
    public async Task SaveAsync_Should_Persist_Venue_Without_Address()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Infrastructure Create Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                null));

        var port = new CreateVenuePort(dbContext);

        await port.SaveAsync(
            venue,
            CancellationToken.None);

        var persistedVenue =
            await dbContext.Venues
                .SingleAsync(
                    item => item.Id == venue.Id);

        Assert.Null(
            persistedVenue.Location.Address);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
