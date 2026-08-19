using System.Net;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments;

public sealed class GetTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTournament_Should_Return_Ok_When_Tournament_Exists()
    {
        using var scope = CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/tournaments/{tournament.Id.Value}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            tournament.Id.Value.ToString(),
            responseBody);

        Assert.Contains(
            tournament.Name,
            responseBody);
    }

    [Fact]
    public async Task GetTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var tournamentId = TournamentId.New();

        var response = await client.GetAsync(
            $"/tournaments/{tournamentId.Value}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private IServiceScope CreateScope()
    {
        return _factory.Services.CreateScope();
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            "API Get Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "API Get Tournament Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "API Get Tournament Test",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
