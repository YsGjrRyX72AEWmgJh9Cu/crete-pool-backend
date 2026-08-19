using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;

public sealed record DeleteMatchCommand(
    MatchId Id);
