using System.Net;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Matches.DeleteMatch;

public sealed class DeleteMatchEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DeleteMatchEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteMatch_Should_Return_NoContent_And_Delete_Match()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var match = new Match(
            MatchId.New(),
            data.Tournament.Id,
            data.Participant1.Id,
            data.Participant2.Id,
            data.Participant1.Id,
            5,
            3);

        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/matches/{match.Id.Value}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var deletedMatch = await dbContext.Matches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == match.Id);

        Assert.Null(deletedMatch);
    }

    [Fact]
    public async Task DeleteMatch_Should_Return_NotFound_When_Match_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/matches/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Delete Match API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Delete Match API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Delete Match API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player1 = Player.Create(
            "Delete Match API",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Delete Match API",
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

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(
            player1,
            player2);

        dbContext.Participations.AddRange(
            participant1,
            participant2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2);
}
