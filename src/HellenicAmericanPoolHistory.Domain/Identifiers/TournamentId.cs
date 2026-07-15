namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Strongly typed identifier for a tournament.
/// </summary>
public readonly record struct TournamentId(Guid Value)
{
    /// <summary>
    /// Creates a new unique tournament identifier.
    /// </summary>
    /// <returns>A new <see cref="TournamentId"/>.</returns>
    public static TournamentId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}