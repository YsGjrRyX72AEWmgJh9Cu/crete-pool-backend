using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Represents the unique identifier of a player.
/// </summary>
public sealed class PlayerId : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerId"/> class.
    /// </summary>
    /// <param name="value">The player identifier.</param>
    public PlayerId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Creates a new unique player identifier.
    /// </summary>
    public static PlayerId New()
    {
        return new PlayerId(Guid.NewGuid());
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString();
    }
}