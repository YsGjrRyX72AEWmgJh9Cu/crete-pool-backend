using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Players.GetPlayer;

public sealed class GetPlayerEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetPlayerEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPlayer_Should_Return_Ok_And_Player()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var player = Player.Create(
            $"API Get Player Test {Guid.NewGuid():N}",
            "Player",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/players/{player.Id.Value}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<GetPlayerResponse>();

        Assert.NotNull(responseBody);

        Assert.Equal(
            player.Id.Value,
            responseBody.Id);

        Assert.Equal(
            player.FirstName,
            responseBody.FirstName);

        Assert.Equal(
            player.LastName,
            responseBody.LastName);

        Assert.Equal(
            player.CountryOfOrigin.Value,
            responseBody.CountryOfOrigin);

        Assert.Equal(
            player.BirthDate,
            responseBody.BirthDate);
    }

    [Fact]
    public async Task GetPlayer_When_Player_Does_Not_Exist_Should_Return_NotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/players/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
