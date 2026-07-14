using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.ValueObjects;

/// <summary>
/// Represents the discipline of a pool tournament.
/// </summary>
public sealed class Discipline : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Discipline"/> class.
    /// </summary>
    /// <param name="value">The discipline name.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the value is null, empty or whitespace.
    /// </exception>
    public Discipline(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value.Trim();
    }

    /// <summary>
    /// Gets the discipline value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value;
    }
}