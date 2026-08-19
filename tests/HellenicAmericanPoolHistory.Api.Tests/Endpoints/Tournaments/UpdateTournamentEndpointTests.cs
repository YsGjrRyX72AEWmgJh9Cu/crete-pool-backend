using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments;

public sealed class UpdateTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UpdateTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdateTournament_Should_Return_NoContent_And_Persist_Changes()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var originalVenue = await CreateVenueAsync(
            dbContext,
            "Original Venue");

        var updatedVenue = await CreateVenueAsync(
            dbContext,
            "Updated Venue");

        var tournament = Tournament.Create(
            new TournamentData(
                "Original Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                originalVenue.Id));

        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var request = new UpdateTournamentRequest(
            "Updated Tournament",
            TournamentType.Team,
            BracketType.DoubleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 8, 25),
            new DateOnly(2026, 8, 26),
            updatedVenue.Id.Value);

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/tournaments/{tournament.Id.Value}",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var updatedTournament = await dbContext.Tournaments
            .AsNoTracking()
            .SingleAsync(
                x => x.Id == tournament.Id);

        Assert.Equal(
            request.Name,
            updatedTournament.Name);

        Assert.Equal(
            request.TournamentType,
            updatedTournament.TournamentType);

        Assert.Equal(
            request.BracketType,
            updatedTournament.BracketType);

        Assert.Equal(
            request.GameSet,
            updatedTournament.GameSet);

        Assert.Equal(
            request.StartDate,
            updatedTournament.StartDate);

        Assert.Equal(
            request.EndDate,
            updatedTournament.EndDate);

        Assert.Equal(
            request.VenueId,
            updatedTournament.VenueId.Value);
    }

    [Fact]
    public async Task UpdateTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var request = new UpdateTournamentRequest(
            "Updated Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 25),
            new DateOnly(2026, 8, 26),
            Guid.NewGuid());

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/tournaments/{Guid.NewGuid()}",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateTournament_Should_Return_BadRequest_When_Name_Is_Empty()
    {
        var request = new UpdateTournamentRequest(
            string.Empty,
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 25),
            new DateOnly(2026, 8, 26),
            Guid.NewGuid());

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/tournaments/{Guid.NewGuid()}",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateTournament_Should_Return_BadRequest_When_StartDate_Is_After_EndDate()
    {
        var request = new UpdateTournamentRequest(
            "Updated Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 30),
            new DateOnly(2026, 8, 29),
            Guid.NewGuid());

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/tournaments/{Guid.NewGuid()}",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    private static async Task<Venue> CreateVenueAsync(
        ApplicationDbContext dbContext,
        string name)
    {
        var venue = Venue.Create(
            $"API Update Tournament {name} {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"API Update Tournament {name} Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        return venue;
    }
}
