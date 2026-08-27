using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Players.UpdatePlayer;

public sealed class UpdatePlayerEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UpdatePlayerEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdatePlayer_Should_Return_NoContent_And_Persist_Changes()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var player = Player.Create(
            $"API Update Player Test {Guid.NewGuid():N}",
            "Original",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var request = new UpdatePlayerCommand(
            player.Id,
            "Updated",
            "Player",
            "Cyprus",
            new DateOnly(1995, 5, 10));

        var response = await client.PutAsJsonAsync(
            $"/players/{player.Id.Value}",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var persistedPlayer =
            await dbContext.Players
                .AsNoTracking()
                .SingleAsync(
                    item => item.Id == player.Id);

        Assert.Equal(
            "Updated",
            persistedPlayer.FirstName);

        Assert.Equal(
            "Player",
            persistedPlayer.LastName);

        Assert.Equal(
            "Cyprus",
            persistedPlayer.CountryOfOrigin.Value);

        Assert.Equal(
            new DateOnly(1995, 5, 10),
            persistedPlayer.BirthDate);
    }

    [Fact]
    public async Task UpdatePlayer_Should_Use_Route_Id()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var player = Player.Create(
            $"API Route Id Test {Guid.NewGuid():N}",
            "Original",
            new Country("Greece"),
            null);

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var request = new UpdatePlayerCommand(
            new HellenicAmericanPoolHistory.Domain.Identifiers.PlayerId(
                Guid.NewGuid()),
            "Updated",
            "Player",
            "Greece",
            null);

        var response = await client.PutAsJsonAsync(
            $"/players/{player.Id.Value}",
            request);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var persistedPlayer =
            await dbContext.Players
                .AsNoTracking()
                .SingleAsync(
                    item => item.Id == player.Id);

        Assert.Equal(
            "Updated",
            persistedPlayer.FirstName);
    }

    [Fact]
    public async Task UpdatePlayer_When_Player_Does_Not_Exist_Should_Return_NotFound()
    {
        var client = _factory.CreateClient();

        var playerId = Guid.NewGuid();

        var request = new UpdatePlayerCommand(
            new HellenicAmericanPoolHistory.Domain.Identifiers.PlayerId(
                Guid.NewGuid()),
            "Updated",
            "Player",
            "Greece",
            null);

        var response = await client.PutAsJsonAsync(
            $"/players/{playerId}",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
