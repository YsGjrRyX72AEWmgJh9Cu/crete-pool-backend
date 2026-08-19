using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

/// <summary>
/// Unit tests for the <see cref="Player"/> entity.
/// </summary>
public class PlayerTests
{
    [Fact]
    public void Create_Should_Create_Player()
    {
        // Arrange
        var country = new Country("Greece");

        // Act
        var player = Player.Create(
            "Manos",
            "Menioudakis",
            country);

        // Assert
        Assert.NotEqual(default, player.Id);
        Assert.Equal("Manos", player.FirstName);
        Assert.Equal("Menioudakis", player.LastName);
        Assert.Equal(country, player.CountryOfOrigin);
    }

    [Fact]
    public void Create_Should_Trim_Names()
    {
        // Arrange
        var country = new Country("Greece");

        // Act
        var player = Player.Create(
            "  Manos  ",
            "  Menioudakis  ",
            country);

        // Assert
        Assert.Equal("Manos", player.FirstName);
        Assert.Equal("Menioudakis", player.LastName);
    }

    [Fact]
    public void Create_Should_Throw_When_FirstName_Is_Empty()
    {
        // Arrange
        var country = new Country("Greece");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Player.Create(
                "",
                "Menioudakis",
                country));
    }

    [Fact]
    public void Create_Should_Throw_When_LastName_Is_Empty()
    {
        // Arrange
        var country = new Country("Greece");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Player.Create(
                "Manos",
                "",
                country));
    }

    [Fact]
    public void Create_Should_Throw_When_Country_Is_Null()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            Player.Create(
                "Manos",
                "Menioudakis",
                null!));
    }
}