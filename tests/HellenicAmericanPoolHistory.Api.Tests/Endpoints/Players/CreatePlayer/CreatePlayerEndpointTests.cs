using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Players.CreatePlayer;

public sealed class CreatePlayerEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreatePlayerEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreatePlayer_Should_Return_Created_And_Player_Id()
    {
        var client = _factory.CreateClient();

        var firstName =
            $"API Create Player Test {Guid.NewGuid():N}";

        var command = new CreatePlayerCommand(
            firstName,
            "Player",
            "Greece",
            new DateOnly(1990, 1, 1));

        var response = await client.PostAsJsonAsync(
            "/players",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<CreatePlayerResponse>();

        Assert.NotNull(responseBody);
        Assert.NotEqual(
            Guid.Empty,
            responseBody.Id);

        Assert.Equal(
            $"/players/{responseBody.Id}",
            response.Headers.Location?.ToString());

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var player = await dbContext.Players
            .AsNoTracking()
            .SingleAsync(player =>
                player.FirstName == firstName);

        Assert.Equal(
            firstName,
            player.FirstName);

        Assert.Equal(
            "Player",
            player.LastName);

        Assert.Equal(
            "Greece",
            player.CountryOfOrigin.Value);

        Assert.Equal(
            new DateOnly(1990, 1, 1),
            player.BirthDate);
    }

    [Fact]
    public async Task CreatePlayer_Should_Return_ValidationProblem_When_FirstName_Is_Empty()
    {
        var client = _factory.CreateClient();

        var command = new CreatePlayerCommand(
            string.Empty,
            "Player",
            "Greece",
            new DateOnly(1990, 1, 1));

        var response = await client.PostAsJsonAsync(
            "/players",
            command);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
