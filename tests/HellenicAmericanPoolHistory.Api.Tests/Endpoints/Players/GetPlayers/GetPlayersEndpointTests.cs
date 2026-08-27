using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Players.GetPlayers;

public sealed class GetPlayersEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetPlayersEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPlayers_Should_Return_Ok_And_Players()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var uniquePrefix =
            $"API Get Players Test {Guid.NewGuid():N}";

        var playerB = Player.Create(
            $"{uniquePrefix} B",
            "Player B",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        var playerA = Player.Create(
            $"{uniquePrefix} A",
            "Player A",
            new Country("Cyprus"),
            new DateOnly(1985, 5, 10));

        dbContext.Players.Add(playerB);
        dbContext.Players.Add(playerA);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/players");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyList<GetPlayersResponse>>();

        Assert.NotNull(responseBody);

        var testPlayers = responseBody
            .Where(player =>
                player.FirstName.StartsWith(uniquePrefix))
            .ToList();

        Assert.Equal(
            2,
            testPlayers.Count);

        Assert.Contains(
            testPlayers,
            player =>
                player.Id == playerA.Id.Value &&
                player.FirstName == playerA.FirstName &&
                player.LastName == playerA.LastName &&
                player.Country == playerA.CountryOfOrigin.Value &&
                player.BirthDate == playerA.BirthDate);

        Assert.Contains(
            testPlayers,
            player =>
                player.Id == playerB.Id.Value &&
                player.FirstName == playerB.FirstName &&
                player.LastName == playerB.LastName &&
                player.Country == playerB.CountryOfOrigin.Value &&
                player.BirthDate == playerB.BirthDate);
    }
}
