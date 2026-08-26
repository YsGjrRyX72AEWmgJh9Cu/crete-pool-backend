using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Domain.Tournament;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Tournaments;

public sealed class CreateTournamentEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateTournamentEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateTournament_Should_Return_Created_And_Persist_Tournament()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var venue = Venue.Create(
            $"API Create Tournament Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "API Create Tournament Test Address"));

        var organization = Organization.Create(
            $"API Create Tournament Organization {Guid.NewGuid():N}");

        var tournamentSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"API Create Tournament Series {Guid.NewGuid():N}");

        dbContext.Venues.Add(venue);
        dbContext.Organizations.Add(organization);
        dbContext.TournamentSeries.Add(tournamentSeries);

        await dbContext.SaveChangesAsync();

        var command = new CreateTournamentCommand(
            "API Create Tournament Test",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 20),
            new DateOnly(2026, 8, 21),
            venue.Id.Value,
            tournamentSeries.Id.Value);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/tournaments",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content.ReadFromJsonAsync<CreateTournamentResponse>();

        Assert.NotNull(responseBody);

        Assert.NotEqual(
            Guid.Empty,
            responseBody.TournamentId);

        Assert.Equal(
            $"/tournaments/{responseBody.TournamentId}",
            response.Headers.Location?.OriginalString);

        var tournament = await dbContext.Tournaments
            .AsNoTracking()
            .SingleOrDefaultAsync(
                tournament => tournament.Id
                    == new TournamentId(responseBody.TournamentId));

        Assert.NotNull(tournament);

        Assert.Equal(
            command.Name,
            tournament.Name);

        Assert.Equal(
            command.TournamentType,
            tournament.TournamentType);

        Assert.Equal(
            command.BracketType,
            tournament.BracketType);

        Assert.Equal(
            command.GameSet,
            tournament.GameSet);

        Assert.Equal(
            command.StartDate,
            tournament.StartDate);

        Assert.Equal(
            command.EndDate,
            tournament.EndDate);

        Assert.Equal(
            command.VenueId,
            tournament.VenueId.Value);

        Assert.Equal(
            command.TournamentSeriesId,
            tournament.TournamentSeriesId?.Value);

        Assert.Equal(
            TournamentStatus.Draft,
            tournament.TournamentStatus);
    }

    [Fact]
    public async Task CreateTournament_Should_Return_BadRequest_When_Name_Is_Empty()
    {
        var command = new CreateTournamentCommand(
            string.Empty,
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 20),
            new DateOnly(2026, 8, 21),
            Guid.NewGuid(),
            null);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/tournaments",
            command);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
