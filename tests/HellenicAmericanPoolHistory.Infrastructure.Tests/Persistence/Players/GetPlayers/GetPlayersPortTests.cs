using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.GetPlayers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Players.GetPlayers;

public sealed class GetPlayersPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAsync_Should_Return_Player()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            $"Get Players Test {Guid.NewGuid():N}",
            "Player",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var port = new GetPlayersPort(dbContext);

        var result = await port.GetAsync(
            CancellationToken.None);

        var response = Assert.Single(
            result,
            item => item.Id == player.Id.Value);

        Assert.Equal(
            player.Id.Value,
            response.Id);

        Assert.Equal(
            player.FirstName,
            response.FirstName);

        Assert.Equal(
            player.LastName,
            response.LastName);

        Assert.Equal(
            player.CountryOfOrigin.Value,
            response.Country);

        Assert.Equal(
            player.BirthDate,
            response.BirthDate);
    }

    [Fact]
    public async Task GetAsync_Should_Return_All_Players()
    {
        await using var dbContext = CreateDbContext();

        var playerA = Player.Create(
            $"Get Players Test A {Guid.NewGuid():N}",
            "Player A",
            new Country("Greece"));

        var playerB = Player.Create(
            $"Get Players Test B {Guid.NewGuid():N}",
            "Player B",
            new Country("Cyprus"),
            new DateOnly(1985, 5, 10));

        dbContext.Players.Add(playerA);
        dbContext.Players.Add(playerB);

        await dbContext.SaveChangesAsync();

        var port = new GetPlayersPort(dbContext);

        var result = await port.GetAsync(
            CancellationToken.None);

        Assert.Contains(
            result,
            player => player.Id == playerA.Id.Value);

        Assert.Contains(
            result,
            player => player.Id == playerB.Id.Value);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
