using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Participations.GetParticipation;

public sealed class GetParticipationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetParticipationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetParticipation_Should_Return_Ok_With_Participation()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/participations/{data.Participation.Id.Value}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<GetParticipationResponse>();

        Assert.NotNull(result);

        Assert.Equal(
            data.Participation.Id.Value,
            result!.Id);

        Assert.Equal(
            data.Player.Id.Value,
            result.PlayerId);

        Assert.Equal(
            "Get Participation API Player One",
            result.PlayerName);

        Assert.Equal(
            data.Tournament.Id.Value,
            result.TournamentId);

        Assert.Equal(
            "Get Participation API Tournament",
            result.TournamentName);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            result.RegistrationDate);

        Assert.Equal(
            3,
            result.Seed);

        Assert.Equal(
            data.Participation.Status.ToString(),
            result.Status);
    }

    [Fact]
    public async Task GetParticipation_Should_Return_NotFound_When_Participation_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/participations/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Participation API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Participation API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Participation API Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Get Participation API",
            "Player One",
            new Country("Greece"));

        var participation = Participation.Create(
            player.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

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
