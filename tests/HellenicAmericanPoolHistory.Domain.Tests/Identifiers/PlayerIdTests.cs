using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Tests.Identifiers;

/// <summary>
/// Unit tests for the <see cref="PlayerId"/> value object.
/// </summary>
public class PlayerIdTests
{
    [Fact]
    public void PlayerIds_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var id = Guid.NewGuid();

        var first = new PlayerId(id);
        var second = new PlayerId(id);

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void PlayerIds_With_Different_Value_Should_Not_Be_Equal()
    {
        // Arrange
        var first = PlayerId.New();
        var second = PlayerId.New();

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void New_Should_Create_Non_Empty_Guid()
    {
        // Arrange
        var id = PlayerId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, id.Value);
    }
}