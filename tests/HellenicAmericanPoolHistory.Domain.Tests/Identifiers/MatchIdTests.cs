using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Tests.Identifiers;

/// <summary>
/// Unit tests for the <see cref="MatchId"/>.
/// </summary>
public class MatchIdTests
{
    [Fact]
    public void MatchIds_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var id = Guid.NewGuid();

        var first = new MatchId(id);
        var second = new MatchId(id);

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void MatchIds_With_Different_Value_Should_Not_Be_Equal()
    {
        // Arrange
        var first = MatchId.New();
        var second = MatchId.New();

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void New_Should_Create_Non_Empty_Guid()
    {
        // Arrange
        var id = MatchId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, id.Value);
    }
}