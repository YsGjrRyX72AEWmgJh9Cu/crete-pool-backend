using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Matches.GetMatches;

public sealed class GetMatchesEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetMatchesEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMatches_Should_Return_Ok_With_Matches()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.GetAsync("/matches");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyCollection<GetMatchesResponse>>();

        Assert.NotNull(result);

        var match = result!.Single(
            item => item.Id == data.Match.Id.Value);

        Assert.Equal(
            data.Tournament.Id.Value,
            match.TournamentId);

        Assert.Equal(
            "Get Matches API Tournament",
            match.TournamentName);

        Assert.Equal(
            data.Participant1.Id.Value,
            match.Participant1Id);

        Assert.Equal(
            "Get Matches API Player One",
            match.Participant1PlayerName);

        Assert.Equal(
            data.Participant2.Id.Value,
            match.Participant2Id);

        Assert.Equal(
            "Get Matches API Player Two",
            match.Participant2PlayerName);

        Assert.Equal(
            data.Participant1.Id.Value,
            match.WinnerParticipationId);

        Assert.Equal(
            "Get Matches API Player One",
            match.WinnerPlayerName);

        Assert.Equal(5, match.Participant1Score);
        Assert.Equal(3, match.Participant2Score);
    }

    [Fact]
    public async Task GetMatches_Should_Return_Ok_With_Empty_Collection_When_No_Matches_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/matches");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyCollection<GetMatchesResponse>>();

        Assert.NotNull(result);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Matches API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Matches API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Get Matches API Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player1 = Player.Create(
            "Get Matches API",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Get Matches API",
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
