using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Domain.Venue;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Venues.UpdateVenue;

public sealed class UpdateVenueEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UpdateVenueEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdateVenue_Should_Return_NoContent_And_Persist_Changes()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = Venue.Create(
            $"API Update Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Original Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var request = new
        {
            Name = "Updated Venue",
            City = "Chania",
            Address = "Updated Address"
        };

        var response = await client.PutAsJsonAsync(
            $"/venues/{venue.Id.Value}",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var persistedVenue =
            await dbContext.Venues
                .AsNoTracking()
                .SingleAsync(
                    item => item.Id ==
                        new VenueId(venue.Id.Value));

        Assert.Equal(
            "Updated Venue",
            persistedVenue.Name);

        Assert.Equal(
            "Greece",
            persistedVenue.Location.Country);

        Assert.Equal(
            "Chania",
            persistedVenue.Location.City);

        Assert.Equal(
            "Updated Address",
            persistedVenue.Location.Address);
    }

    [Fact]
    public async Task UpdateVenue_When_Venue_Does_Not_Exist_Should_Return_NotFound()
    {
        var client = _factory.CreateClient();

        var venueId = Guid.NewGuid();

        var request = new
        {
            Name = "Updated Venue",
            City = "Chania",
            Address = "Updated Address"
        };

        var response = await client.PutAsJsonAsync(
            $"/venues/{venueId}",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
