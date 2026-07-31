using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;

/// <summary>
/// Represents a request to retrieve a player by identifier.
/// </summary>
/// <param name="PlayerId">The player identifier.</param>
public sealed record GetPlayerQuery(PlayerId PlayerId);