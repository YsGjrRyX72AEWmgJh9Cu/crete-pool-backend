namespace HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;

/// <summary>
/// Defines the contract for retrieving matches.
/// </summary>
public interface IGetMatchesPort
{
    /// <summary>
    /// Retrieves all matches.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of matches.</returns>
    Task<IReadOnlyCollection<GetMatchesResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}
