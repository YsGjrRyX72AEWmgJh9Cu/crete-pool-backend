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

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments.CompleteTournament;

public sealed class CompleteTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CompleteTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CompleteTournament_Should_Return_NoContent_And_Complete_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTournamentWithCompletedFinalAsync(
            dbContext);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/complete",
            content: null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        await dbContext.Entry(data.Tournament).ReloadAsync();

        Assert.Equal(
            HellenicAmericanPoolHistory.Domain.Tournament.TournamentStatus.Completed,
            data.Tournament.TournamentStatus);
    }

    [Fact]
    public async Task CompleteTournament_Should_Return_Conflict_When_Final_Is_Not_Completed()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTournamentWithFinalAsync(
            dbContext);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{data.Tournament.Id.Value}/complete",
            content: null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task CompleteTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var tournamentId = Guid.NewGuid();

        var response = await client.PostAsync(
            $"/tournaments/{tournamentId}/complete",
            content: null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CompleteTournament_Should_Return_Conflict_When_Tournament_Is_Not_InProgress()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var tournament = await CreateTournamentAsync(dbContext);

        var client = _factory.CreateClient();

        var response = await client.PostAsync(
            $"/tournaments/{tournament.Id.Value}/complete",
            content: null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData>
        CreateTournamentWithCompletedFinalAsync(
            ApplicationDbContext dbContext)
    {
        var data = await CreateTournamentWithFinalAsync(
            dbContext);

        data.Match.RecordResult(
            data.Participant1.Id,
            5,
            3);

        await dbContext.SaveChangesAsync();

        return data;
    }

    private static async Task<TestData>
        CreateTournamentWithFinalAsync(
            ApplicationDbContext dbContext)
    {
        var tournament = await CreateInProgressTournamentAsync(
            dbContext);

        var player1 = Player.Create(
            "Complete API Final Player 1",
            $"final1-{Guid.NewGuid():N}@test.com",
            new Country("Greece"),
            null);

        var player2 = Player.Create(
            "Complete API Final Player 2",
            $"final2-{Guid.NewGuid():N}@test.com",
            new Country("Greece"),
            null);

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
            match,
            participant1,
            participant2);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos")
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Complete API Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Complete API Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Complete API Test Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }

    private static async Task<Tournament>
        CreateInProgressTournamentAsync(
            ApplicationDbContext dbContext)
    {
        var tournament = await CreateTournamentAsync(
            dbContext);

        tournament.Schedule();
        tournament.Start();

        await dbContext.SaveChangesAsync();

        return tournament;
    }

    private sealed record TestData(
        Tournament Tournament,
        Match Match,
        Participation Participant1,
        Participation Participant2);
}
