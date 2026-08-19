using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;

/// <summary>
/// Represents a request to retrieve a match by identifier.
/// </summary>
/// <param name="MatchId">The match identifier.</param>
public sealed record GetMatchQuery(
    MatchId MatchId);
