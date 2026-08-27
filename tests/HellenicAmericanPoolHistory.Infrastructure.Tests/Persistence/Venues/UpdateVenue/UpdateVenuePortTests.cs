using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.UpdateVenue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Venues.UpdateVenue;

public sealed class UpdateVenuePortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task UpdateAsync_Should_Update_Venue()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Update Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Original Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var data = new VenueData(
            "Updated Venue",
            "Chania",
            "Updated Address");

        var port = new UpdateVenuePort(dbContext);

        await port.UpdateAsync(
            venue.Id,
            data,
            CancellationToken.None);

        await dbContext.Entry(venue).ReloadAsync();

        Assert.Equal(
            "Updated Venue",
            venue.Name);

        Assert.Equal(
            "Greece",
            venue.Location.Country);

        Assert.Equal(
            "Chania",
            venue.Location.City);

        Assert.Equal(
            "Updated Address",
            venue.Location.Address);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Venue_Without_Address()
    {
        await using var dbContext = CreateDbContext();

        var venue = Venue.Create(
            $"Update Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Original Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var data = new VenueData(
            "Updated Venue",
            "Chania",
            string.Empty);

        var port = new UpdateVenuePort(dbContext);

        await port.UpdateAsync(
            venue.Id,
            data,
            CancellationToken.None);

        await dbContext.Entry(venue).ReloadAsync();

        Assert.Null(
            venue.Location.Address);
    }

    [Fact]
    public async Task UpdateAsync_When_Venue_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new UpdateVenuePort(dbContext);

        var data = new VenueData(
            "Updated Venue",
            "Chania",
            "Updated Address");

        var exception =
            await Assert.ThrowsAsync<NotFoundException>(
                () => port.UpdateAsync(
                    new VenueId(Guid.NewGuid()),
                    data,
                    CancellationToken.None));

        Assert.Equal(
            "Venue not found.",
            exception.Message);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
