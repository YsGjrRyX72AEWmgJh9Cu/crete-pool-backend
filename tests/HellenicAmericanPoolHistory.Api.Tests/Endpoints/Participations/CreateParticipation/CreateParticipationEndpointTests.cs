using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Participations.CreateParticipation;

public sealed class CreateParticipationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateParticipationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateParticipation_Should_Return_Created_And_Persist_Participation()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var command = new CreateParticipationCommand(
            data.Player.Id.Value,
            data.Tournament.Id.Value,
            new DateOnly(2026, 8, 18),
            3);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/participations",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content.ReadFromJsonAsync<CreateParticipationResponse>();

        Assert.NotNull(responseBody);

        Assert.NotEqual(
            Guid.Empty,
            responseBody.Id);

        Assert.Equal(
            $"/participations/{responseBody.Id}",
            response.Headers.Location?.OriginalString);

        var participation = await dbContext.Participations
            .AsNoTracking()
            .SingleOrDefaultAsync(
                participation => participation.Id
                    == new ParticipationId(responseBody.Id));

        Assert.NotNull(participation);

        Assert.Equal(
            command.PlayerId,
            participation.PlayerId.Value);

        Assert.Equal(
            command.TournamentId,
            participation.TournamentId.Value);

        Assert.Equal(
            command.RegistrationDate,
            participation.RegistrationDate);

        Assert.Equal(
            command.Seed,
            participation.Seed);
    }

    [Fact]
    public async Task CreateParticipation_Should_Return_BadRequest_When_PlayerId_Is_Empty()
    {
        var command = new CreateParticipationCommand(
            Guid.Empty,
            Guid.NewGuid(),
            new DateOnly(2026, 8, 18),
            1);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/participations",
            command);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateParticipation_Should_Return_NotFound_When_Player_Does_Not_Exist()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var command = new CreateParticipationCommand(
            Guid.NewGuid(),
            data.Tournament.Id.Value,
            new DateOnly(2026, 8, 18),
            1);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/participations",
            command);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateParticipation_Should_Return_NotFound_When_Tournament_Does_Not_Exist()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var command = new CreateParticipationCommand(
            data.Player.Id.Value,
            Guid.NewGuid(),
            new DateOnly(2026, 8, 18),
            1);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/participations",
            command);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateParticipation_Should_Return_Conflict_When_Player_Is_Already_Registered()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var data = await CreateTestDataAsync(dbContext);

        var existingParticipation = Participation.Create(
            data.Player.Id,
            data.Tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        dbContext.Participations.Add(existingParticipation);

        await dbContext.SaveChangesAsync();

        var command = new CreateParticipationCommand(
            data.Player.Id.Value,
            data.Tournament.Id.Value,
            new DateOnly(2026, 8, 18),
            2);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/participations",
            command);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            $"Create Participation API Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Create Participation API Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Create Participation API Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        var player = Player.Create(
            "Create Participation API",
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
