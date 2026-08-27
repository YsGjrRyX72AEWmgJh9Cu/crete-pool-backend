using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.UpdatePlayer;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Players.UpdatePlayer;

public sealed class UpdatePlayerPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task UpdateAsync_Should_Update_Player()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            $"Update Player Test {Guid.NewGuid():N}",
            "Original",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var command = new UpdatePlayerCommand(
            player.Id,
            "Updated",
            "Player",
            "Cyprus",
            new DateOnly(1995, 5, 10));

        var port = new UpdatePlayerPort(dbContext);

        await port.UpdateAsync(
            command,
            CancellationToken.None);

        await dbContext.Entry(player).ReloadAsync();

        Assert.Equal(
            "Updated",
            player.FirstName);

        Assert.Equal(
            "Player",
            player.LastName);

        Assert.Equal(
            "Cyprus",
            player.CountryOfOrigin.Value);

        Assert.Equal(
            new DateOnly(1995, 5, 10),
            player.BirthDate);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Player_Without_BirthDate()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            $"Update Player Test {Guid.NewGuid():N}",
            "Original",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var command = new UpdatePlayerCommand(
            player.Id,
            "Updated",
            "Player",
            "Greece",
            null);

        var port = new UpdatePlayerPort(dbContext);

        await port.UpdateAsync(
            command,
            CancellationToken.None);

        await dbContext.Entry(player).ReloadAsync();

        Assert.Null(
            player.BirthDate);
    }

    [Fact]
    public async Task UpdateAsync_When_Player_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new UpdatePlayerPort(dbContext);

        var command = new UpdatePlayerCommand(
            new PlayerId(Guid.NewGuid()),
            "Updated",
            "Player",
            "Greece",
            null);

        var exception =
            await Assert.ThrowsAsync<NotFoundException>(
                () => port.UpdateAsync(
                    command,
                    CancellationToken.None));

        Assert.Equal(
            "Player not found.",
            exception.Message);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
