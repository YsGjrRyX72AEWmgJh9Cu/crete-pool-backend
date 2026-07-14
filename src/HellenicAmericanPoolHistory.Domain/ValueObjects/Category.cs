using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.ValueObjects;

/// <summary>
/// Represents the category of a pool tournament.
/// </summary>
public sealed class Category : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="value">The category name.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the value is null, empty or whitespace.
    /// </exception>
    public Category(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value.Trim();
    }

    /// <summary>
    /// Gets the category value.
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