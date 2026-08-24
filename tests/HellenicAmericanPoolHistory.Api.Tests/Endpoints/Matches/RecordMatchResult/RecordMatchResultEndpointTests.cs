using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Api.Endpoints.Matches;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Matches.RecordMatchResult;

public sealed class RecordMatchResultEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RecordMatchResultEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RecordMatchResult_Should_Return_NoContent_And_Persist_Result()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: true);

        var client = _factory.CreateClient();

        var request = new RecordMatchResultRequest(
            data.Participant1.Id.Value,
            5,
            3);

        var response = await client.PostAsJsonAsync(
            $"/matches/{data.Match.Id.Value}/result",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(data.Match).ReloadAsync();

        Assert.Equal(
            data.Participant1.Id,
            data.Match.WinnerParticipationId);

        Assert.Equal(
            5,
            data.Match.Participant1Score);

        Assert.Equal(
            3,
            data.Match.Participant2Score);
    }

    [Fact]
    public async Task RecordMatchResult_Should_Return_BadRequest_When_Score_Is_Negative()
    {
        var client = _factory.CreateClient();

        var request = new RecordMatchResultRequest(
            Guid.NewGuid(),
            -1,
            3);

        var response = await client.PostAsJsonAsync(
            $"/matches/{Guid.NewGuid()}/result",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task RecordMatchResult_Should_Return_NotFound_When_Match_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var request = new RecordMatchResultRequest(
            Guid.NewGuid(),
            5,
            3);

        var response = await client.PostAsJsonAsync(
            $"/matches/{Guid.NewGuid()}/result",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task RecordMatchResult_Should_Return_Conflict_When_Tournament_Is_Not_InProgress()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: false);

        var client = _factory.CreateClient();

        var request = new RecordMatchResultRequest(
            data.Participant1.Id.Value,
            5,
            3);

        var response = await client.PostAsJsonAsync(
            $"/matches/{data.Match.Id.Value}/result",
            request);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext,
        bool startTournament)
    {
        var venue = Venue.Create(
            $"Record Match Result API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Record Match Result API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Record Match Result API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        if (startTournament)
        {
            tournament.Schedule();
            tournament.Start();
        }

        var player1 = Player.Create(
            "Record Match Result API",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Record Match Result API",
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

        participant1.Update(
            1,
            ParticipationStatus.CheckedIn);

        participant2.Update(
            2,
            ParticipationStatus.CheckedIn);

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant2.Id);

        dbContext.Venues.Add(venue);

        dbContext.Tournaments.Add(tournament);

        dbContext.Players.AddRange(
            player1,
            player2);

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
