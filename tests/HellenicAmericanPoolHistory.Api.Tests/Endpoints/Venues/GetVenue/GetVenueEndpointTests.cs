using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Venues.GetVenue;

public sealed class GetVenueEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetVenueEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetVenue_Should_Return_Ok_And_Venue()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = Venue.Create(
            $"API Get Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/venues/{venue.Id.Value}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<GetVenueResponse>();

        Assert.NotNull(responseBody);

        Assert.Equal(
            venue.Id.Value,
            responseBody.Id);

        Assert.Equal(
            venue.Name,
            responseBody.Name);

        Assert.Equal(
            venue.Location.Country,
            responseBody.Country);

        Assert.Equal(
            venue.Location.City,
            responseBody.City);

        Assert.Equal(
            venue.Location.Address,
            responseBody.Address);
    }

    [Fact]
    public async Task GetVenue_When_Venue_Does_Not_Exist_Should_Return_NotFound()
    {
        var client = _factory.CreateClient();

        var venueId = Guid.NewGuid();

        var response = await client.GetAsync(
            $"/venues/{venueId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
