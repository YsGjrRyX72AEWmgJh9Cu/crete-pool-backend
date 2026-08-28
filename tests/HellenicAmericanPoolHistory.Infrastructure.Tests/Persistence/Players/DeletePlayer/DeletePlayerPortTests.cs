using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.DeletePlayer;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Players.DeletePlayer;

public sealed class DeletePlayerPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task DeleteAsync_Should_Delete_Player()
    {
        await using var dbContext = CreateDbContext();

        var player = Player.Create(
            $"Delete Player Test {Guid.NewGuid():N}",
            "Player",
            new Country("Greece"),
            new DateOnly(1990, 1, 1));

        dbContext.Players.Add(player);

        await dbContext.SaveChangesAsync();

        var command = new DeletePlayerCommand(
            player.Id);

        var port = new DeletePlayerPort(dbContext);

        await port.DeleteAsync(
            command,
            CancellationToken.None);

        var persistedPlayer =
            await dbContext.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    item => item.Id == player.Id);

        Assert.Null(persistedPlayer);
    }

    [Fact]
    public async Task DeleteAsync_When_Player_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new DeletePlayerPort(dbContext);

        var command = new DeletePlayerCommand(
            new HellenicAmericanPoolHistory.Domain.Identifiers.PlayerId(
                Guid.NewGuid()));

        var exception =
            await Assert.ThrowsAsync<NotFoundException>(
                () => port.DeleteAsync(
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
