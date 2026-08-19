using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments;

public sealed class GetTournamentsEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetTournamentsEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTournaments_Should_Return_Ok_With_Tournaments_Ordered_By_StartDate_And_Name()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var firstTournament = await CreateTournamentAsync(
            dbContext,
            "B Tournament",
            new DateOnly(2026, 8, 16));

        var secondTournament = await CreateTournamentAsync(
            dbContext,
            "A Tournament",
            new DateOnly(2026, 8, 14));

        var thirdTournament = await CreateTournamentAsync(
            dbContext,
            "A Tournament",
            new DateOnly(2026, 8, 16));

        var client = _factory.CreateClient();

        var response = await client.GetAsync("/tournaments");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var tournaments =
            await response.Content.ReadFromJsonAsync<
                IReadOnlyList<GetTournamentsResponse>>();

        Assert.NotNull(tournaments);

        var returned = tournaments!;

        var firstIndex = returned
            .Select(t => t.Id)
            .ToList()
            .IndexOf(secondTournament.Id.Value);

        var secondIndex = returned
            .Select(t => t.Id)
            .ToList()
            .IndexOf(thirdTournament.Id.Value);

        var thirdIndex = returned
            .Select(t => t.Id)
            .ToList()
            .IndexOf(firstTournament.Id.Value);

        Assert.True(firstIndex >= 0);
        Assert.True(secondIndex >= 0);
        Assert.True(thirdIndex >= 0);

        Assert.True(firstIndex < secondIndex);
        Assert.True(secondIndex < thirdIndex);
    }

    [Fact]
    public async Task GetTournaments_Should_Return_Ok_When_No_Tournaments_Exist()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/tournaments");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var tournaments =
            await response.Content.ReadFromJsonAsync<
                IReadOnlyList<GetTournamentsResponse>>();

        Assert.NotNull(tournaments);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext,
        string name,
        DateOnly startDate)
    {
        var venue = Venue.Create(
            $"API Get Tournaments Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "API Get Tournaments Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                name,
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                startDate,
                startDate,
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }
}
