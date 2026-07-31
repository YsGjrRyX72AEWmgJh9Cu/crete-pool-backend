using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.CreatePlayer;

/// <summary>
/// Persists newly created players.
/// </summary>
public sealed class CreatePlayerPort : ICreatePlayerPort
{
    private readonly ApplicationDbContext _dbContext;

    public CreatePlayerPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<PlayerId> CreateAsync(
        Player player,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Players.Add(player);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return player.Id;
    }
}