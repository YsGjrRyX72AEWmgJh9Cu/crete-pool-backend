using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Tests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Country"/> value object.
/// </summary>
public class CountryTests
{
    /// <summary>
    /// Verifies that two countries with the same value are equal.
    /// </summary>
    [Fact]
    public void Countries_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var first = new Country("Greece");
        var second = new Country("Greece");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that two countries with different values are not equal.
    /// </summary>
    [Fact]
    public void Countries_With_Different_Values_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new Country("Greece");
        var second = new Country("USA");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that creating a country with an empty value throws an exception.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_Value_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => new Country(string.Empty));
    }

    /// <summary>
    /// Verifies that leading and trailing whitespace is removed.
    /// </summary>
    [Fact]
    public void Constructor_Should_Trim_Value()
    {
        // Arrange
        var country = new Country("  Greece  ");

        // Assert
        Assert.Equal("Greece", country.Value);
    }
}