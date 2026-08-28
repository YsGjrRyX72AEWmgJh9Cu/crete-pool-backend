using System.Net;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Players.DeletePlayer;

public sealed class DeletePlayerEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DeletePlayerEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeletePlayer_Should_Return_NoContent_And_Delete_Player()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var player = Player.Create(
            $"API Delete Player Test {Guid.NewGuid():N}",
            "Player",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/players/{player.Id.Value}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var persistedPlayer =
            await dbContext.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    item => item.Id == player.Id);

        Assert.Null(persistedPlayer);
    }

    [Fact]
    public async Task DeletePlayer_When_Player_Does_Not_Exist_Should_Return_NotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync(
            $"/players/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
