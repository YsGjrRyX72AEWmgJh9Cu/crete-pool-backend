namespace HellenicAmericanPoolHistory.Domain.ValueObjects;

using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

/// <summary>
/// Represents the official country associated with a domain concept.
/// </summary>
public sealed class Country : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Country"/> class.
    /// </summary>
    /// <param name="value">The official country name.</param>
    public Country(string value)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(value);

    Value = value.Trim();
}

    /// <summary>
    /// Gets the official country name.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}