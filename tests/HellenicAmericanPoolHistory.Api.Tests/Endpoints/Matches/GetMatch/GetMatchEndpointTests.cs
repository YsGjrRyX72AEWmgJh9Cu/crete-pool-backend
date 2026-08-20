using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Matches.GetMatch;

public sealed class GetMatchEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetMatchEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMatch_Should_Return_Ok_With_Match_Data()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/matches/{data.Match.Id.Value}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<GetMatchResponse>();

        Assert.NotNull(result);

        Assert.Equal(
            data.Match.Id.Value,
            result!.Id);

        Assert.Equal(
            data.Tournament.Id.Value,
            result.TournamentId);

        Assert.Equal(
            "Get Match API Tournament",
            result.TournamentName);

        Assert.Equal(
            data.Participant1.Id.Value,
            result.Participant1Id);

        Assert.Equal(
            "Get Match API Player One",
            result.Participant1PlayerName);

        Assert.Equal(
            data.Participant2.Id.Value,
            result.Participant2Id);

        Assert.Equal(
            "Get Match API Player Two",
            result.Participant2PlayerName);

        Assert.Equal(
            data.Participant1.Id.Value,
            result.WinnerParticipationId);

        Assert.Equal(
            "Get Match API Player One",
            result.WinnerPlayerName);

        Assert.Equal(5, result.Participant1Score);
        Assert.Equal(3, result.Participant2Score);
    }

    [Fact]
    public async Task GetMatch_Should_Return_NotFound_When_Match_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/matches/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Match API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Match API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Match API Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player1 = Player.Create(
            "Get Match API",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Get Match API",
            "Player Two",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            2);

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            participant1.Id,
            participant2.Id);

        match.RecordResult(
            participant1.Id,
            5,
            3);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(player1, player2);
        dbContext.Participations.AddRange(
            participant1,
            participant2);
        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2,
            match);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2,
        Match Match);
}
