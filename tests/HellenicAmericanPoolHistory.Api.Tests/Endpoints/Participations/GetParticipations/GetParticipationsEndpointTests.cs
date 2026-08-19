using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Participations.GetParticipations;

public sealed class GetParticipationsEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetParticipationsEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetParticipations_Should_Return_Ok_With_Participations()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/participations");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<
                IReadOnlyCollection<GetParticipationsResponse>>();

        Assert.NotNull(result);

        var participation1 = Assert.Single(
            result!.Where(
                x => x.Id == data.Participation1.Id.Value));

        Assert.Equal(
            "Get Participations API Player One",
            participation1.PlayerName);

        Assert.Equal(
            "Get Participations API Tournament",
            participation1.TournamentName);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            participation1.RegistrationDate);

        Assert.Equal(
            1,
            participation1.Seed);

        Assert.Equal(
            data.Participation1.Status.ToString(),
            participation1.Status);

        var participation2 = Assert.Single(
            result.Where(
                x => x.Id == data.Participation2.Id.Value));

        Assert.Equal(
            "Get Participations API Player Two",
            participation2.PlayerName);

        Assert.Equal(
            "Get Participations API Tournament",
            participation2.TournamentName);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            participation2.RegistrationDate);

        Assert.Equal(
            2,
            participation2.Seed);

        Assert.Equal(
            data.Participation2.Status.ToString(),
            participation2.Status);

        var returnedIds = result
            .Select(x => x.Id)
            .ToList();

        var participation1Index =
            returnedIds.IndexOf(data.Participation1.Id.Value);

        var participation2Index =
            returnedIds.IndexOf(data.Participation2.Id.Value);

        Assert.True(
            participation1Index < participation2Index);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Participations API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Participations API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Participations API Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player1 = Player.Create(
            "Get Participations API",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Get Participations API",
            "Player Two",
            new Country("Greece"));

        var participation1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        var participation2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            2);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        dbContext.Players.AddRange(
            player1,
            player2);

        dbContext.Participations.AddRange(
            participation1,
            participation2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            player1,
            player2,
            participation1,
            participation2);
    }

    private sealed record TestData(
        Tournament Tournament,
        Player Player1,
        Player Player2,
        Participation Participation1,
        Participation Participation2);
}
