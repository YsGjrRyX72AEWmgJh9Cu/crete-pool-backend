using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.GetVenue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Venues.GetVenue;

public sealed class GetVenuePortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByIdAsync_Should_Return_Venue()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Get Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var port = new GetVenuePort(dbContext);

        var result = await port.GetByIdAsync(
            venue.Id.Value,
            CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(
            venue.Id.Value,
            result.Id);

        Assert.Equal(
            venue.Name,
            result.Name);

        Assert.Equal(
            venue.Location.Country,
            result.Country);

        Assert.Equal(
            venue.Location.City,
            result.City);

        Assert.Equal(
            venue.Location.Address,
            result.Address);
    }

    [Fact]
    public async Task GetByIdAsync_When_Venue_Does_Not_Exist_Should_Return_Null()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetVenuePort(dbContext);

        var result = await port.GetByIdAsync(
            Guid.NewGuid(),
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
}
