using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;

/// <summary>
/// Defines the contract for retrieving a player.
/// </summary>
public interface IGetPlayerPort
{
    /// <summary>
    /// Retrieves a player by identifier.
    /// </summary>
    /// <param name="playerId">The player identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The player if found; otherwise <see langword="null" />.
    /// </returns>
    Task<GetPlayerResponse?> GetByIdAsync(
        PlayerId playerId,
        CancellationToken cancellationToken);
}