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

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments;

public sealed class DeleteTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DeleteTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteTournament_Should_Return_NoContent_And_Delete_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = await CreateVenueAsync(
            dbContext,
            "Delete Tournament Venue");

        var tournament = Tournament.Create(
            new TournamentData(
                "Delete Tournament Test",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/tournaments/{tournament.Id.Value}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var deletedTournament = await dbContext.Tournaments
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.Id == tournament.Id);

        Assert.Null(deletedTournament);
    }

    [Fact]
    public async Task DeleteTournament_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        var client = _factory.CreateClient();

        var tournamentId = TournamentId.New();

        var response = await client.DeleteAsync(
            $"/tournaments/{tournamentId.Value}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteTournament_Should_Return_Conflict_When_Tournament_Has_Participations()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = await CreateVenueAsync(
            dbContext,
            "Delete Tournament Participation Venue");

        var tournament = Tournament.Create(
            new TournamentData(
                "Delete Tournament With Participation",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        var player = Player.Create(
            "Delete Tournament Test Player",
            "delete-tournament-player@example.com",
            new Country("Greece"),
            new DateOnly(2026, 8, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var participation = Participation.Create(
            player.Id,
            tournament.Id,
            new DateOnly(2026, 8, 1),
            null);

        dbContext.Participations.Add(participation);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/tournaments/{tournament.Id.Value}");

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);

        var existingTournament = await dbContext.Tournaments
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.Id == tournament.Id);

        Assert.NotNull(existingTournament);
    }

    private static async Task<Venue> CreateVenueAsync(
        ApplicationDbContext dbContext,
        string name)
    {
        var venue = Venue.Create(
            $"API {name} {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"API {name} Address"));

        dbContext.Venues.Add(venue);

        await dbContext.SaveChangesAsync();

        return venue;
    }
}
