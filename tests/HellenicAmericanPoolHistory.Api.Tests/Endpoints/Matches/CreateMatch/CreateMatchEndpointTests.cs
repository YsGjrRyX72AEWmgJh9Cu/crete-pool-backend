using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Matches.CreateMatch;

public sealed class CreateMatchEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateMatchEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateMatch_Should_Return_Created_And_Persist_Match_Without_Result()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var command = new CreateMatchCommand(
            data.Tournament.Id.Value,
            data.Participant1.Id.Value,
            data.Participant2.Id.Value,
            null,
            null,
            null);

        var response = await client.PostAsJsonAsync(
            "/matches",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<CreateMatchResponse>();

        Assert.NotNull(result);

        var match = await dbContext.Matches.FindAsync(
            new MatchId(result!.Id));

        Assert.NotNull(match);

        Assert.Equal(
            data.Tournament.Id,
            match!.TournamentId);

        Assert.Equal(
            data.Participant1.Id,
            match.Participant1Id);

        Assert.Equal(
            data.Participant2.Id,
            match.Participant2Id);

        Assert.Null(match.WinnerParticipationId);
        Assert.Null(match.Participant1Score);
        Assert.Null(match.Participant2Score);
    }

    [Fact]
    public async Task CreateMatch_Should_Return_Created_And_Persist_Match_With_Result()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var command = new CreateMatchCommand(
            data.Tournament.Id.Value,
            data.Participant1.Id.Value,
            data.Participant2.Id.Value,
            data.Participant1.Id.Value,
            5,
            3);

        var response = await client.PostAsJsonAsync(
            "/matches",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<CreateMatchResponse>();

        Assert.NotNull(result);

        var match = await dbContext.Matches.FindAsync(
            new MatchId(result!.Id));

        Assert.NotNull(match);

        Assert.Equal(
            data.Tournament.Id,
            match!.TournamentId);

        Assert.Equal(
            data.Participant1.Id,
            match.Participant1Id);

        Assert.Equal(
            data.Participant2.Id,
            match.Participant2Id);

        Assert.Equal(
            data.Participant1.Id,
            match.WinnerParticipationId);

        Assert.Equal(
            5,
            match.Participant1Score);

        Assert.Equal(
            3,
            match.Participant2Score);
    }

    [Fact]
    public async Task CreateMatch_Should_Return_BadRequest_When_Score_Is_Negative()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            -1,
            3);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/matches",
            command);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateMatch_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            data.Participant1.Id.Value,
            data.Participant2.Id.Value,
            data.Participant1.Id.Value,
            5,
            3);

        var response = await client.PostAsJsonAsync(
            "/matches",
            command);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateMatch_Should_Return_NotFound_When_Participation_Does_Not_Exist()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var command = new CreateMatchCommand(
            data.Tournament.Id.Value,
            data.Participant1.Id.Value,
            Guid.NewGuid(),
            data.Participant1.Id.Value,
            5,
            3);

        var response = await client.PostAsJsonAsync(
            "/matches",
            command);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateMatch_Should_Return_Conflict_When_Participant_Belongs_To_Another_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var client = _factory.CreateClient();

        var command = new CreateMatchCommand(
            data.Tournament.Id.Value,
            data.Participant1.Id.Value,
            data.OtherTournamentParticipant.Id.Value,
            data.Participant1.Id.Value,
            5,
            3);

        var response = await client.PostAsJsonAsync(
            "/matches",
            command);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Create Match API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Create Match API Address {Guid.NewGuid():N}"));

        var otherVenue = Venue.Create(
            $"Create Match API Other Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Create Match API Other Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Create Match API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var otherTournament = Tournament.Create(
            new TournamentData(
                $"Create Match API Other Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                otherVenue.Id));

        var player1 = Player.Create(
            "Create Match API",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Create Match API",
            "Player Two",
            new Country("Greece"));

        var player3 = Player.Create(
            "Create Match API",
            "Player Three",
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

        var otherTournamentParticipant = Participation.Create(
            player3.Id,
            otherTournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Venues.AddRange(
            venue,
            otherVenue);

        dbContext.Tournaments.AddRange(
            tournament,
            otherTournament);

        dbContext.Players.AddRange(
            player1,
            player2,
            player3);

        dbContext.Participations.AddRange(
            participant1,
            participant2,
            otherTournamentParticipant);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2,
            otherTournamentParticipant);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2,
        Participation OtherTournamentParticipant);
}
