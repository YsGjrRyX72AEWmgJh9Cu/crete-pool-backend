using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.GetVenues;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Venues.GetVenues;

public sealed class GetVenuesPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_Venues_Ordered_By_Name()
    {
        await using var dbContext = CreateDbContext();

        var uniquePrefix =
            $"Get Venues Test {Guid.NewGuid():N}";

        var venueB = Venue.Create(
            $"{uniquePrefix} B",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Address B"));

        var venueA = Venue.Create(
            $"{uniquePrefix} A",
            new VenueLocation(
                "Greece",
                "Chania",
                "Address A"));

        dbContext.Venues.Add(venueB);
        dbContext.Venues.Add(venueA);

        await dbContext.SaveChangesAsync();

        var port = new GetVenuesPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        var testVenues = result
            .Where(venue =>
                venue.Name.StartsWith(uniquePrefix))
            .ToList();

        Assert.Equal(
            2,
            testVenues.Count);

        Assert.Equal(
            venueA.Id.Value,
            testVenues[0].Id);

        Assert.Equal(
            venueA.Name,
            testVenues[0].Name);

        Assert.Equal(
            venueB.Id.Value,
            testVenues[1].Id);

        Assert.Equal(
            venueB.Name,
            testVenues[1].Name);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Venues()
    {
        await using var dbContext = CreateDbContext();

        var uniquePrefix =
            $"Get Venues Test {Guid.NewGuid():N}";

        var venueA = Venue.Create(
            $"{uniquePrefix} A",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Address A"));

        var venueB = Venue.Create(
            $"{uniquePrefix} B",
            new VenueLocation(
                "Cyprus",
                "Nicosia",
                "Address B"));

        dbContext.Venues.Add(venueA);
        dbContext.Venues.Add(venueB);

        await dbContext.SaveChangesAsync();

        var port = new GetVenuesPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        Assert.Contains(
            result,
            venue => venue.Id == venueA.Id.Value);

        Assert.Contains(
            result,
            venue => venue.Id == venueB.Id.Value);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
