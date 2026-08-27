using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.CreatePlayer;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Players.CreatePlayer;

public sealed class CreatePlayerPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task CreateAsync_Should_Persist_Player_And_Return_Id()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            "Create",
            "Player",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        var port = new CreatePlayerPort(dbContext);

        var result = await port.CreateAsync(
            player,
            CancellationToken.None);

        Assert.Equal(
            player.Id.Value,
            result.Value);

        await using var verificationDbContext = CreateDbContext();

        var persistedPlayer =
            await verificationDbContext.Players
                .AsNoTracking()
                .SingleAsync(x => x.Id == player.Id);

        Assert.Equal(
            player.Id.Value,
            persistedPlayer.Id.Value);

        Assert.Equal(
            "Create",
            persistedPlayer.FirstName);

        Assert.Equal(
            "Player",
            persistedPlayer.LastName);

        Assert.Equal(
            "Greece",
            persistedPlayer.CountryOfOrigin.Value);

        Assert.Equal(
            new DateOnly(1990, 1, 1),
            persistedPlayer.BirthDate);
    }

    [Fact]
    public async Task CreateAsync_Should_Persist_Player_Without_BirthDate()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            "Create",
            "NoBirthDate",
            new Country("Greece"),
            null);

        var port = new CreatePlayerPort(dbContext);

        var result = await port.CreateAsync(
            player,
            CancellationToken.None);

        Assert.Equal(
            player.Id.Value,
            result.Value);

        await using var verificationDbContext = CreateDbContext();

        var persistedPlayer =
            await verificationDbContext.Players
                .AsNoTracking()
                .SingleAsync(x => x.Id == player.Id);

        Assert.Null(persistedPlayer.BirthDate);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
