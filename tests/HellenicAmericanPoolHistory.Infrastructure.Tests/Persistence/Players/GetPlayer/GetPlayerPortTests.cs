using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.GetPlayer;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Players.GetPlayer;

public sealed class GetPlayerPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByIdAsync_Should_Return_Player()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            $"Get Player Test {Guid.NewGuid():N}",
            "Player",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var port = new GetPlayerPort(dbContext);

        var result = await port.GetByIdAsync(
            player.Id,
            CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(
            player.Id.Value,
            result.Id);

        Assert.Equal(
            player.FirstName,
            result.FirstName);

        Assert.Equal(
            player.LastName,
            result.LastName);

        Assert.Equal(
            player.CountryOfOrigin.Value,
            result.CountryOfOrigin);

        Assert.Equal(
            player.BirthDate,
            result.BirthDate);
    }

    [Fact]
    public async Task GetByIdAsync_When_Player_Does_Not_Exist_Should_Return_Null()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetPlayerPort(dbContext);

        var result = await port.GetByIdAsync(
            new HellenicAmericanPoolHistory.Domain.Identifiers.PlayerId(
                Guid.NewGuid()),
            CancellationToken.None);

        Assert.Null(result);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
