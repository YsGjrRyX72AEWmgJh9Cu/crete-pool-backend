namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Strongly typed identifier for a player.
/// </summary>
public readonly record struct PlayerId(Guid Value)
{
    /// <summary>
    /// Creates a new unique player identifier.
    /// </summary>
    /// <returns>A new <see cref="PlayerId"/>.</returns>
    public static PlayerId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}