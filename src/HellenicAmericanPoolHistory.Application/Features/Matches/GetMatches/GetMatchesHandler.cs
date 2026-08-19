namespace HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;

/// <summary>
/// Handles requests to retrieve matches.
/// </summary>
public sealed class GetMatchesHandler(
    IGetMatchesPort port)
{
    /// <summary>
    /// Handles the request.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of matches.</returns>
    public Task<IReadOnlyCollection<GetMatchesResponse>> HandleAsync(
        GetMatchesQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetAllAsync(cancellationToken);
    }
}
