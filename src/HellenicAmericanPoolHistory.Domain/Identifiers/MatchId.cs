namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Strongly typed identifier for a match.
/// </summary>
public readonly record struct MatchId(Guid Value)
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