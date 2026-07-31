using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

public class TournamentTests
{
    [Fact]
    public void Constructor_Should_Create_Tournament()
    {
        // Arrange
        var id = TournamentId.New();

        // Act
        var tournament = new Tournament(
            id,
            "Crete Open",
            new Country("Greece"),
            new Discipline("8-Ball"),
            new Category("Open"),
            new DateOnly(2025, 5, 1),
            new DateOnly(2025, 5, 3));

        // Assert
        Assert.Equal(id, tournament.Id);
        Assert.Equal("Crete Open", tournament.Name);
        Assert.Equal(new Country("Greece"), tournament.Country);
        Assert.Equal(new Discipline("8-Ball"), tournament.Discipline);
        Assert.Equal(new Category("Open"), tournament.Category);
    }

    [Fact]
    public void Constructor_Should_Throw_When_EndDate_Is_Before_StartDate()
    {
        Assert.Throws<ArgumentException>(() =>
            new Tournament(
                TournamentId.New(),
                "Crete Open",
                new Country("Greece"),
                new Discipline("8-Ball"),
                new Category("Open"),
                new DateOnly(2025, 5, 5),
                new DateOnly(2025, 5, 1)));
    }
}