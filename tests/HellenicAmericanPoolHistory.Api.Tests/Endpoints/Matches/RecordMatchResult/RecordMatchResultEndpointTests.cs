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
using Microsoft.EntityFrameworkCore;
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
            $"/matches/{data.Match1.Id.Value}/result",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(data.Match1).ReloadAsync();

        Assert.Equal(
            data.Participant1.Id,
            data.Match1.WinnerParticipationId);

        Assert.Equal(
            5,
            data.Match1.Participant1Score);

        Assert.Equal(
            3,
            data.Match1.Participant2Score);
    }

    [Fact]
    public async Task RecordMatchResult_Should_Create_Next_Round_When_Current_Round_Is_Complete()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: true,
            createSecondMatch: true);

        var client = _factory.CreateClient();

        var firstMatchRequest = new RecordMatchResultRequest(
            data.Participant1.Id.Value,
            5,
            3);

        var firstMatchResponse = await client.PostAsJsonAsync(
            $"/matches/{data.Match1.Id.Value}/result",
            firstMatchRequest);

        Assert.Equal(
            HttpStatusCode.NoContent,
            firstMatchResponse.StatusCode);

        var secondMatchRequest = new RecordMatchResultRequest(
            data.Participant3.Id.Value,
            5,
            2);

        var secondMatchResponse = await client.PostAsJsonAsync(
            $"/matches/{data.Match2!.Id.Value}/result",
            secondMatchRequest);

        Assert.Equal(
            HttpStatusCode.NoContent,
            secondMatchResponse.StatusCode);

        var nextRoundMatches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 2)
            .ToListAsync();

        Assert.Single(nextRoundMatches);

        var nextRoundMatch = nextRoundMatches[0];

        Assert.Equal(
            data.Participant1.Id,
            nextRoundMatch.Participant1Id);

        Assert.Equal(
            data.Participant3.Id,
            nextRoundMatch.Participant2Id);

        Assert.Equal(
            2,
            nextRoundMatch.Round);

        Assert.Equal(
            1,
            nextRoundMatch.BracketPosition);
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
            $"/matches/{data.Match1.Id.Value}/result",
            request);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext,
        bool startTournament,
        bool createSecondMatch = false)
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

        var player3 = Player.Create(
            "Record Match Result API",
            "Player Three",
            new Country("Greece"));

        var player4 = Player.Create(
            "Record Match Result API",
            "Player Four",
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

        var participant3 = Participation.Create(
            player3.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

        var participant4 = Participation.Create(
            player4.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            4);

        participant1.Update(
            1,
            ParticipationStatus.CheckedIn);

        participant2.Update(
            2,
            ParticipationStatus.CheckedIn);

        participant3.Update(
            3,
            ParticipationStatus.CheckedIn);

        participant4.Update(
            4,
            ParticipationStatus.CheckedIn);

        var match1 = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant2.Id);

        Match? match2 = null;

        if (createSecondMatch)
        {
            match2 = new Match(
                MatchId.New(),
                tournament.Id,
                1,
                2,
                participant3.Id,
                participant4.Id);
        }

        dbContext.Venues.Add(venue);

        dbContext.Tournaments.Add(tournament);

        dbContext.Players.AddRange(
            player1,
            player2,
            player3,
            player4);

        dbContext.Participations.AddRange(
            participant1,
            participant2,
            participant3,
            participant4);

        dbContext.Matches.Add(match1);

        if (match2 is not null)
        {
            dbContext.Matches.Add(match2);
        }

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2,
            participant3,
            participant4,
            match1,
            match2);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2,
        Participation Participant3,
        Participation Participant4,
        Match Match1,
        Match? Match2);
}
