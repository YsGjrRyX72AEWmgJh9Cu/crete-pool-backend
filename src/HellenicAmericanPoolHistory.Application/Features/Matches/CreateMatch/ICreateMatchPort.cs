using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

/// <summary>
/// Defines the persistence operations for creating a match.
/// </summary>
public interface ICreateMatchPort
{
    /// <summary>
    /// Creates a new match.
    /// </summary>
    /// <param name="match">The match to persist.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created match identifier.</returns>
    Task<MatchId> CreateAsync(
        Match match,
        CancellationToken cancellationToken = default);
}
