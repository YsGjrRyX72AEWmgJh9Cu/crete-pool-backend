using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Tests.Identifiers;

/// <summary>
/// Unit tests for the <see cref="ParticipationId"/> value object.
/// </summary>
public class ParticipationIdTests
{
    [Fact]
    public void ParticipationIds_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var id = Guid.NewGuid();

        var first = new ParticipationId(id);
        var second = new ParticipationId(id);

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ParticipationIds_With_Different_Value_Should_Not_Be_Equal()
    {
        // Arrange
        var first = ParticipationId.New();
        var second = ParticipationId.New();

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void New_Should_Create_Non_Empty_Guid()
    {
        // Arrange
        var id = ParticipationId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, id.Value);
    }
}