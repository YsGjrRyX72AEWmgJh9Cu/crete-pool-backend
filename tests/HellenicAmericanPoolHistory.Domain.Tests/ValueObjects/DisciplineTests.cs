using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Tests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Discipline"/> value object.
/// </summary>
public class DisciplineTests
{
    [Fact]
    public void Disciplines_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var first = new Discipline("8-Ball");
        var second = new Discipline("8-Ball");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Disciplines_With_Different_Values_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new Discipline("8-Ball");
        var second = new Discipline("9-Ball");

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Value_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => new Discipline(string.Empty));
    }

    [Fact]
    public void Constructor_Should_Trim_Value()
    {
        // Arrange
        var discipline = new Discipline("  8-Ball  ");

        // Assert
        Assert.Equal("8-Ball", discipline.Value);
    }
}