using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Tests.Common.Abstractions;

/// <summary>
/// Unit tests for <see cref="ValueObject"/>.
/// </summary>
public class ValueObjectTests
{
    private sealed class TestValueObject : ValueObject
    {
        public TestValueObject(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    /// <summary>
    /// Verifies that two value objects with the same components are equal.
    /// </summary>
    [Fact]
    public void ValueObjects_With_Same_Components_Should_Be_Equal()
    {
        // Arrange
        var first = new TestValueObject("Greece");
        var second = new TestValueObject("Greece");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that two value objects with different components are not equal.
    /// </summary>
    [Fact]
    public void ValueObjects_With_Different_Components_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new TestValueObject("Greece");
        var second = new TestValueObject("USA");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that equal value objects produce the same hash code.
    /// </summary>
    [Fact]
    public void Equal_ValueObjects_Should_Have_Same_HashCode()
    {
        // Arrange
        var first = new TestValueObject("Greece");
        var second = new TestValueObject("Greece");

        // Act
        var firstHash = first.GetHashCode();
        var secondHash = second.GetHashCode();

        // Assert
        Assert.Equal(firstHash, secondHash);
    }
}