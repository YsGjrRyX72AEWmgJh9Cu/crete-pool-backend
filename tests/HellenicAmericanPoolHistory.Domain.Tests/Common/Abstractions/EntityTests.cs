using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Tests.Common.Abstractions;

public class EntityTests
{
    private sealed class TestEntity : Entity<Guid>
    {
        public TestEntity(Guid id) : base(id)
        {
        }
    }

    [Fact]
    public void Entities_With_Same_Id_Should_Be_Equal()
    {
        // Arrange
        var id = Guid.NewGuid();

        var first = new TestEntity(id);
        var second = new TestEntity(id);

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Entities_With_Different_Id_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new TestEntity(Guid.NewGuid());
        var second = new TestEntity(Guid.NewGuid());

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equality_Operator_Should_Return_True_For_Same_Id()
    {
        // Arrange
        var id = Guid.NewGuid();

        var first = new TestEntity(id);
        var second = new TestEntity(id);

        // Act
        var result = first == second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Inequality_Operator_Should_Return_True_For_Different_Id()
    {
        // Arrange
        var first = new TestEntity(Guid.NewGuid());
        var second = new TestEntity(Guid.NewGuid());

        // Act
        var result = first != second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Entity_Should_Not_Be_Equal_To_NonEntity()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity.Equals(new object());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Entity_Should_Be_Equal_To_Same_Reference()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity.Equals(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Entities_With_Same_Id_Should_Have_Same_HashCode()
    {
        // Arrange
        var id = Guid.NewGuid();

        var first = new TestEntity(id);
        var second = new TestEntity(id);

        // Act
        var firstHash = first.GetHashCode();
        var secondHash = second.GetHashCode();

        // Assert
        Assert.Equal(firstHash, secondHash);
    }
}
