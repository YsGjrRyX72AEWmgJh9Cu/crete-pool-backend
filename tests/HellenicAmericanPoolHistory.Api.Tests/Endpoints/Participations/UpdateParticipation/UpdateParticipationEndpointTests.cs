using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Api.Endpoints.Participations;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Participations.UpdateParticipation;

public sealed class UpdateParticipationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UpdateParticipationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdateParticipation_Should_Return_NoContent_And_Update_Participation()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var request = new UpdateParticipationRequest(
            5,
            HellenicAmericanPoolHistory.Domain.Enums.ParticipationStatus.Withdrawn);

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/participations/{data.Participation.Id.Value}",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(data.Participation).ReloadAsync();

        Assert.Equal(
            5,
            data.Participation.Seed);

        Assert.Equal(
            HellenicAmericanPoolHistory.Domain.Enums.ParticipationStatus.Withdrawn,
            data.Participation.Status);
    }

    [Fact]
    public async Task UpdateParticipation_Should_Return_NotFound_When_Participation_Does_Not_Exist()
    {
        var request = new UpdateParticipationRequest(
            5,
            HellenicAmericanPoolHistory.Domain.Enums.ParticipationStatus.Withdrawn);

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/participations/{Guid.NewGuid()}",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateParticipation_Should_Return_BadRequest_When_Seed_Is_Invalid()
    {
        var request = new UpdateParticipationRequest(
            -1,
            HellenicAmericanPoolHistory.Domain.Enums.ParticipationStatus.Withdrawn);

        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/participations/{Guid.NewGuid()}",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Update Participation API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Update Participation API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Update Participation API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Update Participation API",
            "Player",
            new Country("Greece"));

        var participation = Participation.Create(
            player.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.Add(player);
        dbContext.Participations.Add(participation);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            player,
            participation);
    }

    private sealed record TestData(
        Tournament Tournament,
        Player Player,
        Participation Participation);
}
