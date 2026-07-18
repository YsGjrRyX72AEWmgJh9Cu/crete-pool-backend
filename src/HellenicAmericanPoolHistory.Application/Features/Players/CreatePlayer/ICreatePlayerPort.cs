using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;

/// <summary>
/// Defines the persistence operations required to create a player.
/// </summary>
public interface ICreatePlayerPort
{
    Task<PlayerId> CreateAsync(
        Player player,
        CancellationToken cancellationToken = default);
}