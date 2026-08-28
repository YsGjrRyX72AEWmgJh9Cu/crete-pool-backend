using System.Net;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Venues.DeleteVenue;

public sealed class DeleteVenueEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DeleteVenueEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteVenue_Should_Return_NoContent_And_Delete_Venue()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = Venue.Create(
            $"API Delete Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/venues/{venue.Id.Value}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var persistedVenue =
            await dbContext.Venues
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    item => item.Id == venue.Id);

        Assert.Null(persistedVenue);
    }

    [Fact]
    public async Task DeleteVenue_When_Venue_Does_Not_Exist_Should_Return_NotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/venues/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteVenue_When_Venue_Is_Used_By_Tournament_Should_Return_Conflict()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = Venue.Create(
            $"API Delete Venue Test {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Test Address"));

        dbContext.Venues.Add(venue);

        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        var tournament = Tournament.Create(
            new TournamentData(
                "API Delete Venue Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo7,
                date,
                date,
                venue.Id,
                null));

        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/venues/{venue.Id.Value}");

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);

        Assert.True(
            await dbContext.Venues
                .AsNoTracking()
                .AnyAsync(
                    item => item.Id == venue.Id));
    }
}
