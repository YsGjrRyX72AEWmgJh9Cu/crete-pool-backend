using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;

/// <summary>
/// Defines the contract for retrieving a match.
/// </summary>
public interface IGetMatchPort
{
    /// <summary>
    /// Retrieves a match by identifier.
    /// </summary>
    /// <param name="matchId">The match identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The match if found; otherwise <see langword="null" />.
    /// </returns>
    Task<GetMatchResponse?> GetByIdAsync(
        MatchId matchId,
        CancellationToken cancellationToken);
}
