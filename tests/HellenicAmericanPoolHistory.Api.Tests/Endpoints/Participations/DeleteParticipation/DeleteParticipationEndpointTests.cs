using System.Net;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Participations.DeleteParticipation;

public sealed class DeleteParticipationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DeleteParticipationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteParticipation_Should_Return_NoContent_And_Delete_Participation()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var participation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Participations.Add(participation);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/participations/{participation.Id.Value}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var deletedParticipation = await dbContext.Participations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == participation.Id);

        Assert.Null(deletedParticipation);
    }

    [Fact]
    public async Task DeleteParticipation_Should_Return_NotFound_When_Participation_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/participations/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Delete Participation API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Delete Participation API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Delete Participation API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Delete Participation API",
            "Player One",
            new Country("Greece"));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            player);
    }

    private sealed record TestData(
        Tournament Tournament,
        Player Player);
}
