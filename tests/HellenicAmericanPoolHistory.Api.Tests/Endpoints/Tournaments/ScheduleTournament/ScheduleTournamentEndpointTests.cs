using System.Net;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments.ScheduleTournament;

public sealed class ScheduleTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ScheduleTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ScheduleTournament_Should_Return_NoContent_And_Schedule_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/schedule",
            content: null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.Scheduled,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task ScheduleTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var tournamentId = Guid.NewGuid();

        var response = await client.PostAsync(
            $"/tournaments/{tournamentId}/schedule",
            content: null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task ScheduleTournament_Should_Return_Conflict_When_Tournament_Is_Not_Draft()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/schedule",
            content: null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Schedule API Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Schedule API Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Schedule API Test Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
