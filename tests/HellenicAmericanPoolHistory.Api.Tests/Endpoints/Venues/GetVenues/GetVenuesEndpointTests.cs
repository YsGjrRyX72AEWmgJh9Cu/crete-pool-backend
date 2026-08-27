using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Venues.GetVenues;

public sealed class GetVenuesEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetVenuesEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetVenues_Should_Return_Ok_And_Venues_Ordered_By_Name()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var uniquePrefix =
            $"API Get Venues Test {Guid.NewGuid():N}";

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

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/venues");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyList<GetVenuesResponse>>();

        Assert.NotNull(responseBody);

        var testVenues = responseBody
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
    public async Task GetVenues_When_No_Matching_Venues_Exist_Should_Return_Ok_And_Empty_Filter_Result()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var uniquePrefix =
            $"API Get Venues Empty Test {Guid.NewGuid():N}";

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/venues");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyList<GetVenuesResponse>>();

        Assert.NotNull(responseBody);

        Assert.DoesNotContain(
            responseBody,
            venue => venue.Name.StartsWith(uniquePrefix));
    }
}
