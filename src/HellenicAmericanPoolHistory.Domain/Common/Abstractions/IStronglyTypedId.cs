namespace HellenicAmericanPoolHistory.Domain.Common.Abstractions;

/// <summary>
/// Represents a strongly typed identifier.
/// </summary>
/// <typeparam name="TValue">The underlying value type.</typeparam>
public interface IStronglyTypedId<out TValue>
{
    /// <summary>
    /// Gets the underlying value.
    /// </summary>
    TValue Value { get; }
}