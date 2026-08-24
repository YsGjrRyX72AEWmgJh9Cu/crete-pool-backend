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
    .GenerateTournamentBracket;

public sealed class GenerateTournamentBracketEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GenerateTournamentBracketEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_NoContent_And_Create_First_Round()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: true);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var matches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id)
            .ToListAsync();

        Assert.Equal(2, matches.Count);

        var firstMatch = matches.Single(
            match =>
                match.Participant1Id == data.Participants[0].Id &&
                match.Participant2Id == data.Participants[3].Id);

        var secondMatch = matches.Single(
            match =>
                match.Participant1Id == data.Participants[1].Id &&
                match.Participant2Id == data.Participants[2].Id);

        Assert.Equal(
            data.Tournament.Id,
            firstMatch.TournamentId);

        Assert.Equal(
            data.Tournament.Id,
            secondMatch.TournamentId);

        Assert.Null(firstMatch.WinnerParticipationId);
        Assert.Null(firstMatch.Participant1Score);
        Assert.Null(firstMatch.Participant2Score);

        Assert.Null(secondMatch.WinnerParticipationId);
        Assert.Null(secondMatch.Participant1Score);
        Assert.Null(secondMatch.Participant2Score);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{Guid.NewGuid()}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_Tournament_Is_Not_InProgress()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: false);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_Bracket_Type_Is_Not_Single_Elimination()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: true,
            bracketType:
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.RoundRobin);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_Bracket_Already_Exists()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: true);

        dbContext.Matches.Add(
            new Match(
                MatchId.New(),
                data.Tournament.Id,
                1,
                1,
                data.Participants[0].Id,
                data.Participants[3].Id));

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_There_Are_Less_Than_Two_CheckedIn_Participants()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 1,
            startTournament: true);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_Participant_Count_Is_Not_Power_Of_Two()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 3,
            startTournament: true);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_Participant_Has_No_Seed()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: true);

        data.Participants[3].Update(
            null,
            ParticipationStatus.CheckedIn);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task GenerateTournamentBracket_Should_Return_Conflict_When_Seeds_Are_Not_Unique()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: true);

        data.Participants[3].Update(
            1,
            ParticipationStatus.CheckedIn);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/bracket",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext,
        int participantCount,
        bool startTournament,
        HellenicAmericanPoolHistory.Domain.Tournament.BracketType bracketType =
            HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination)
    {
        var venue = Venue.Create(
            $"Generate Bracket API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Generate Bracket API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Generate Bracket API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                bracketType,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 21),
                new DateOnly(2026, 8, 21),
                venue.Id));

        if (startTournament)
        {
            tournament.Schedule();
            tournament.Start();
        }

        var participants = new List<Participation>();

        for (var index = 0; index < participantCount; index++)
        {
            var player = Player.Create(
                "Generate Bracket API",
                $"Player {index + 1}",
                new Country("Greece"));

            var participation = Participation.Create(
                player.Id,
                tournament.Id,
                new DateOnly(2026, 8, 20),
                index + 1);

            participation.Update(
                index + 1,
                ParticipationStatus.CheckedIn);

            dbContext.Players.Add(player);
            participants.Add(participation);
        }

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Participations.AddRange(participants);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participants);
    }

    private sealed record TestData(
        Tournament Tournament,
        List<Participation> Participants);
}
