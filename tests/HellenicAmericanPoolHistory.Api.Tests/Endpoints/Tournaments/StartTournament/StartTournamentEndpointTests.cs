using System.Net;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments.StartTournament;

public sealed class StartTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public StartTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task StartTournament_Should_Return_NoContent_And_Start_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateScheduledTournamentAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/start",
            content: null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.InProgress,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task StartTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var tournamentId = Guid.NewGuid();

        var response = await client.PostAsync(
            $"/tournaments/{tournamentId}/start",
            content: null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task StartTournament_Should_Return_Conflict_When_Tournament_Is_Not_Scheduled()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/start",
            content: null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Start API Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Start API Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Start API Test Tournament {Guid.NewGuid():N}",
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

    private static async Task<Tournament> CreateScheduledTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
