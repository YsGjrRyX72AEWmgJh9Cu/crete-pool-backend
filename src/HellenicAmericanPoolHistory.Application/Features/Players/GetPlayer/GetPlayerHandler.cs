namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;

/// <summary>
/// Handles requests to retrieve a player.
/// </summary>
public sealed class GetPlayerHandler(IGetPlayerPort port)
{
    /// <summary>
    /// Handles the request.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The player if found; otherwise <see langword="null" />.
    /// </returns>
    public Task<GetPlayerResponse?> HandleAsync(
        GetPlayerQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetByIdAsync(
            query.PlayerId,
            cancellationToken);
    }
}