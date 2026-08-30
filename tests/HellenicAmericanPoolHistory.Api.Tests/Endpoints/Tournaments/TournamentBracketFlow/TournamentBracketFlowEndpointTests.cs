using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Api.Endpoints.Matches;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments
    .TournamentBracketFlow;

public sealed class TournamentBracketFlowEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TournamentBracketFlowEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TournamentBracket_Should_Complete_Four_Player_Single_Elimination_Flow()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        // Generate bracket.
        var generateResponse = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            generateResponse.StatusCode);

        // Verify first round.
        var firstRoundMatches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 1)
            .OrderBy(match => match.BracketPosition)
            .ToListAsync();

        Assert.Equal(
            2,
            firstRoundMatches.Count);

        var match1 = firstRoundMatches[0];
        var match2 = firstRoundMatches[1];

        Assert.Equal(
            data.Participants[0].Id,
            match1.Participant1Id);

        Assert.Equal(
            data.Participants[3].Id,
            match1.Participant2Id);

        Assert.Equal(
            data.Participants[1].Id,
            match2.Participant1Id);

        Assert.Equal(
            data.Participants[2].Id,
            match2.Participant2Id);

        // Record first round results.
        var match1Result = new RecordMatchResultRequest(
            data.Participants[0].Id.Value,
            5,
            3);

        var match1Response = await client.PostAsJsonAsync(
            $"/matches/{match1.Id.Value}/result",
            match1Result);

        Assert.Equal(
            HttpStatusCode.NoContent,
            match1Response.StatusCode);

        var match2Result = new RecordMatchResultRequest(
            data.Participants[2].Id.Value,
            2,
            5);

        var match2Response = await client.PostAsJsonAsync(
            $"/matches/{match2.Id.Value}/result",
            match2Result);

        Assert.Equal(
            HttpStatusCode.NoContent,
            match2Response.StatusCode);

        // The second first-round result automatically advances
        // the bracket and creates the final.

        // Verify final participants.
        var final = await dbContext.Matches
            .SingleAsync(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 2 &&
                match.BracketPosition == 1);

        Assert.Equal(
            data.Participants[0].Id,
            final.Participant1Id);

        Assert.Equal(
            data.Participants[2].Id,
            final.Participant2Id);

        Assert.Null(final.WinnerParticipationId);

        // Record final result.
        var finalResult = new RecordMatchResultRequest(
            data.Participants[0].Id.Value,
            5,
            4);

        var finalResponse = await client.PostAsJsonAsync(
            $"/matches/{final.Id.Value}/result",
            finalResult);

        Assert.Equal(
            HttpStatusCode.NoContent,
            finalResponse.StatusCode);

        // Read the complete bracket through the public API.
        var bracketResponse = await client.GetAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket");

        Assert.Equal(
            HttpStatusCode.OK,
            bracketResponse.StatusCode);

        var bracket =
            await bracketResponse.Content
                .ReadFromJsonAsync<GetTournamentBracketResponse>();

        Assert.NotNull(bracket);

        Assert.Equal(
            data.Tournament.Id.Value,
            bracket.TournamentId);

        Assert.Equal(
            2,
            bracket.Rounds.Count);

        var returnedFinal = bracket.Rounds
            .Single(round => round.Round == 2)
            .Matches
            .Single();

        Assert.Equal(
            final.Id.Value,
            returnedFinal.Id);

        Assert.Equal(
            data.Participants[0].Id.Value,
            returnedFinal.Participant1Id);

        Assert.Equal(
            data.Participants[2].Id.Value,
            returnedFinal.Participant2Id);

        Assert.Equal(
            data.Participants[0].Id.Value,
            returnedFinal.WinnerParticipationId);

        Assert.Equal(
            5,
            returnedFinal.Participant1Score);

        Assert.Equal(
            4,
            returnedFinal.Participant2Score);

        Assert.Equal(
            "Tournament Flow Player tournament-flow-player-1@test.com",
            returnedFinal.WinnerPlayerName);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Tournament Flow API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Tournament Flow API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Tournament Flow API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 30),
                new DateOnly(2026, 8, 30),
                venue.Id));

        tournament.Schedule();
        tournament.Start();

        var participants = new List<Participation>();

        for (var index = 0; index < 4; index++)
        {
            var player = Player.Create(
                "Tournament Flow Player",
                $"tournament-flow-player-{index + 1}@test.com",
                new Country("Greece"));

            var participant = Participation.Create(
                player.Id,
                tournament.Id,
                new DateOnly(2026, 8, 30),
                index + 1);

            participant.Update(
                index + 1,
                ParticipationStatus.CheckedIn);

            dbContext.Players.Add(player);
            dbContext.Participations.Add(participant);

            participants.Add(participant);
        }

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participants);
    }

    private sealed record TestData(
        Tournament Tournament,
        List<Participation> Participants);
}
