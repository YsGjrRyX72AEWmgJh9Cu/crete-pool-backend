using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments.GetTournamentBracket;

public sealed class GetTournamentBracketEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetTournamentBracketEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTournamentBracket_Should_Return_Ok_With_Bracket()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(dbContext);

        var player1 = Player.Create(
            "Bracket API Player 1",
            "bracket-api-player-1@test.com",
            new Country("Greece"));

        var player2 = Player.Create(
            "Bracket API Player 2",
            "bracket-api-player-2@test.com",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 20),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 20),
            2);

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant2.Id);

        match.RecordResult(
            participant1.Id,
            5,
            3);

        dbContext.Players.AddRange(
            player1,
            player2);

        dbContext.Participations.AddRange(
            participant1,
            participant2);

        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/tournaments/{tournament.Id.Value}/bracket");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<GetTournamentBracketResponse>();

        Assert.NotNull(result);

        Assert.Equal(
            tournament.Id.Value,
            result.TournamentId);

        Assert.Equal(
            tournament.Name,
            result.TournamentName);

        var round = Assert.Single(result.Rounds);

        Assert.Equal(
            1,
            round.Round);

        var returnedMatch = Assert.Single(round.Matches);

        Assert.Equal(
            match.Id.Value,
            returnedMatch.Id);

        Assert.Equal(
            1,
            returnedMatch.BracketPosition);

        Assert.Equal(
            participant1.Id.Value,
            returnedMatch.Participant1Id);

        Assert.Equal(
            participant2.Id.Value,
            returnedMatch.Participant2Id);

        Assert.Equal(
            "Bracket API Player 1 bracket-api-player-1@test.com",
            returnedMatch.Participant1PlayerName);

        Assert.Equal(
            "Bracket API Player 2 bracket-api-player-2@test.com",
            returnedMatch.Participant2PlayerName);

        Assert.Equal(
            participant1.Id.Value,
            returnedMatch.WinnerParticipationId);

        Assert.Equal(
            "Bracket API Player 1 bracket-api-player-1@test.com",
            returnedMatch.WinnerPlayerName);

        Assert.Equal(
            5,
            returnedMatch.Participant1Score);

        Assert.Equal(
            3,
            returnedMatch.Participant2Score);
    }

    [Fact]
    public async Task GetTournamentBracket_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/tournaments/{Guid.NewGuid()}/bracket");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Get Tournament Bracket API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Get Tournament Bracket API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Get Tournament Bracket API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
