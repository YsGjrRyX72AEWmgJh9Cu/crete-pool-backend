namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Strongly typed identifier for a participation.
/// </summary>
public readonly record struct ParticipationId(Guid Value)
{
    /// <summary>
    /// Creates a new unique participation identifier.
    /// </summary>
    /// <returns>A new <see cref="ParticipationId"/>.</returns>
    public static ParticipationId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}