namespace HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;

/// <summary>
/// Handles requests to retrieve a match.
/// </summary>
public sealed class GetMatchHandler(
    IGetMatchPort port)
{
    /// <summary>
    /// Handles the request.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The match if found; otherwise <see langword="null" />.
    /// </returns>
    public Task<GetMatchResponse?> HandleAsync(
        GetMatchQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetByIdAsync(
            query.MatchId,
            cancellationToken);
    }
}
