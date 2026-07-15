using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Represents the unique identifier of a participation.
/// </summary>
public sealed class ParticipationId : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParticipationId"/> class.
    /// </summary>
    /// <param name="value">The participation identifier.</param>
    public ParticipationId(Guid value)
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
    /// Creates a new unique participation identifier.
    /// </summary>
    public static ParticipationId New()
    {
        return new ParticipationId(Guid.NewGuid());
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString();
    }
}