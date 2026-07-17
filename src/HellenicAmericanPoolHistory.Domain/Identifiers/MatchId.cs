using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Strongly typed identifier for a match.
/// </summary>
public readonly record struct MatchId(Guid Value)
    : IStronglyTypedId<Guid>
{
    /// <summary>
    /// Creates a new unique match identifier.
    /// </summary>
    /// <returns>A new <see cref="MatchId"/>.</returns>
    public static MatchId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}