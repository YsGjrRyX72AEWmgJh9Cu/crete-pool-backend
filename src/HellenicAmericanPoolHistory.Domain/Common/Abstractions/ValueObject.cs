namespace HellenicAmericanPoolHistory.Domain.Common.Abstractions;

/// <summary>
/// Base class for all domain value objects.
/// Equality is based on the values of their components.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Returns the components that participate in equality comparison.
    /// </summary>
    /// <returns>
    /// A sequence of values that uniquely define the value object.
    /// </returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>
    /// <c>true</c> if the specified object is equal to the current value object; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Returns the hash code for the current value object.
    /// </summary>
    /// <returns>
    /// A hash code based on the equality components.
    /// </returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var component in GetEqualityComponents())
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }
    
    /// <summary>
    /// Determines whether two value objects are equal.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two value objects are not equal.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}