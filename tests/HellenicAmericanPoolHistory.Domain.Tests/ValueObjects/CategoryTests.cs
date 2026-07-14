using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Tests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Category"/> value object.
/// </summary>
public class CategoryTests
{
    [Fact]
    public void Categories_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var first = new Category("Men");
        var second = new Category("Men");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Categories_With_Different_Values_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new Category("Men");
        var second = new Category("Women");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Value_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => new Category(string.Empty));
    }

    [Fact]
    public void Constructor_Should_Trim_Value()
    {
        // Arrange
        var category = new Category("  Men  ");

        // Assert
        Assert.Equal("Men", category.Value);
    }
}