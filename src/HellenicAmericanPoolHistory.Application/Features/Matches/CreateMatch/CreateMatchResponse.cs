namespace HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

/// <summary>
/// Represents the response after creating a match.
/// </summary>
/// <param name="Id">The created match identifier.</param>
public sealed record CreateMatchResponse(
    Guid Id);