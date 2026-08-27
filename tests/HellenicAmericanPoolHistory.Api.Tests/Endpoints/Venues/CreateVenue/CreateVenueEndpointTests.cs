using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Venues.CreateVenue;

public sealed class CreateVenueEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateVenueEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateVenue_Should_Return_Created_And_Persist_Venue()
    {
        var client = _factory.CreateClient();

        var name =
            $"API Create Venue Test {Guid.NewGuid():N}";

        var command = new CreateVenueCommand(
            name,
            "Greece",
            "Heraklion",
            "Test Address");

        var response = await client.PostAsJsonAsync(
            "/venues",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<CreateVenueResponse>();

        Assert.NotNull(responseBody);

        Assert.NotEqual(
            Guid.Empty,
            responseBody.VenueId);

        Assert.Equal(
            $"/venues/{responseBody.VenueId}",
            response.Headers.Location?.ToString());

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venues =
            await dbContext.Venues
                .AsNoTracking()
                .ToListAsync();

        var persistedVenue =
            Assert.Single(
                venues,
                venue => venue.Id.Value == responseBody.VenueId);

        Assert.Equal(
            command.Name,
            persistedVenue.Name);

        Assert.Equal(
            command.Country,
            persistedVenue.Location.Country);

        Assert.Equal(
            command.City,
            persistedVenue.Location.City);

        Assert.Equal(
            command.Address,
            persistedVenue.Location.Address);
    }

    [Fact]
    public async Task CreateVenue_Should_Return_Created_When_Address_Is_Null()
    {
        var client = _factory.CreateClient();

        var command = new CreateVenueCommand(
            $"API Create Venue Test {Guid.NewGuid():N}",
            "Greece",
            "Chania",
            null);

        var response = await client.PostAsJsonAsync(
            "/venues",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<CreateVenueResponse>();

        Assert.NotNull(responseBody);

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venues =
            await dbContext.Venues
                .AsNoTracking()
                .ToListAsync();

        var persistedVenue =
            Assert.Single(
                venues,
                venue => venue.Id.Value == responseBody.VenueId);

        Assert.Null(
            persistedVenue.Location.Address);
    }
}
