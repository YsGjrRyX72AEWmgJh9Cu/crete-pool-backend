using System.Net;
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
    .AdvanceTournamentBracket;

public sealed class AdvanceTournamentBracketEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AdvanceTournamentBracketEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AdvanceTournamentBracket_Should_Return_NoContent_And_Create_Next_Round()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        data.Match2!.RecordResult(
            data.Participant3.Id,
            5,
            2);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket/advance",
            null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

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
    }

    [Fact]
    public async Task AdvanceTournamentBracket_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{TournamentId.New().Value}/bracket/advance",
            null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task AdvanceTournamentBracket_Should_Return_Conflict_When_Current_Round_Is_Not_Complete()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket/advance",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task AdvanceTournamentBracket_Should_Return_Conflict_When_Tournament_Is_Not_InProgress()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: false);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket/advance",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task AdvanceTournamentBracket_Should_Return_Conflict_When_Bracket_Has_Not_Been_Generated()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            createMatches: false);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket/advance",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext,
        bool startTournament = true,
        bool createMatches = true)
    {
        var venue = Venue.Create(
            $"Advance Bracket API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Advance Bracket API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Advance Bracket API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 21),
                new DateOnly(2026, 8, 21),
                venue.Id));

        if (startTournament)
        {
            tournament.Schedule();
            tournament.Start();
        }

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        var participants = new List<Participation>();

        for (var index = 0; index < 4; index++)
        {
            var player = Player.Create(
                "Advance Bracket API",
                $"Player {index + 1}",
                new Country("Greece"));

            var participant = Participation.Create(
                player.Id,
                tournament.Id,
                new DateOnly(2026, 8, 18),
                index + 1);

            participant.Update(
                index + 1,
                ParticipationStatus.CheckedIn);

            dbContext.Players.Add(player);
            dbContext.Participations.Add(participant);

            participants.Add(participant);
        }

        Match? match1 = null;
        Match? match2 = null;

        if (createMatches)
        {
            match1 = new Match(
                MatchId.New(),
                tournament.Id,
                1,
                1,
                participants[0].Id,
                participants[1].Id);

            match2 = new Match(
                MatchId.New(),
                tournament.Id,
                1,
                2,
                participants[2].Id,
                participants[3].Id);

            dbContext.Matches.AddRange(
                match1,
                match2);
        }

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            match1,
            match2,
            participants[0],
            participants[1],
            participants[2],
            participants[3]);
    }

    private sealed record TestData(
        Tournament Tournament,
        Match? Match1,
        Match? Match2,
        Participation Participant1,
        Participation Participant2,
        Participation Participant3,
        Participation Participant4);
}
