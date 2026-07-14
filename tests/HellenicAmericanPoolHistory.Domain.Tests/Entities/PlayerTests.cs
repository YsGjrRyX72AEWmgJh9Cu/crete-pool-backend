using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

/// <summary>
/// Unit tests for the <see cref="Player"/> entity.
/// </summary>
public class PlayerTests
{
    [Fact]
    public void Constructor_Should_Create_Player()
    {
        // Arrange
        var id = PlayerId.New();
        var country = new Country("Greece");

        // Act
        var player = new Player(
            id,
            "Manos",
            "Menioudakis",
            country);

        // Assert
        Assert.Equal(id, player.Id);
        Assert.Equal("Manos", player.FirstName);
        Assert.Equal("Menioudakis", player.LastName);
        Assert.Equal(country, player.Country);
    }

    [Fact]
    public void Constructor_Should_Trim_Names()
    {
        // Arrange
        var id = PlayerId.New();
        var country = new Country("Greece");

        // Act
        var player = new Player(
            id,
            "  Manos  ",
            "  Menioudakis  ",
            country);

        // Assert
        Assert.Equal("Manos", player.FirstName);
        Assert.Equal("Menioudakis", player.LastName);
    }

    [Fact]
    public void Constructor_Should_Throw_When_FirstName_Is_Empty()
    {
        var id = PlayerId.New();
        var country = new Country("Greece");

        Assert.Throws<ArgumentException>(() =>
            new Player(id, "", "Menioudakis", country));
    }

    [Fact]
    public void Constructor_Should_Throw_When_LastName_Is_Empty()
    {
        var id = PlayerId.New();
        var country = new Country("Greece");

        Assert.Throws<ArgumentException>(() =>
            new Player(id, "Manos", "", country));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Country_Is_Null()
    {
        var id = PlayerId.New();

        Assert.Throws<ArgumentNullException>(() =>
            new Player(id, "Manos", "Menioudakis", null!));
    }
}