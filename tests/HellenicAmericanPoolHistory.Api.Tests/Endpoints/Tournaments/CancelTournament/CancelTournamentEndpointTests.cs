using System.Net;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments.CancelTournament;

public sealed class CancelTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CancelTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CancelTournament_Should_Return_NoContent_And_Cancel_Draft_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(
            dbContext,
            "Cancel Draft API Tournament");

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/cancel",
            content: null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.Cancelled,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task CancelTournament_Should_Return_NoContent_And_Cancel_Scheduled_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(
            dbContext,
            "Cancel Scheduled API Tournament");

        tournament.Schedule();

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/cancel",
            content: null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.Cancelled,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task CancelTournament_Should_Return_Conflict_When_Tournament_Is_InProgress()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(
            dbContext,
            "Cancel InProgress API Tournament");

        tournament.Schedule();
        tournament.Start();

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/cancel",
            content: null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task CancelTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var tournamentId = Guid.NewGuid();

        var response = await client.PostAsync(
            $"/tournaments/{tournamentId}/cancel",
            content: null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext,
        string name)
    {
        var venue = Venue.Create(
            $"{name} Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"{name} Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                name,
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 27),
                new DateOnly(2026, 8, 27),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
