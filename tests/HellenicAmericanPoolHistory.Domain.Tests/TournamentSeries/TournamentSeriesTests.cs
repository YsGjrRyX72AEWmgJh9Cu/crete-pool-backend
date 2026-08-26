using HellenicAmericanPoolHistory.Domain.Organization;

using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Domain.Tests.TournamentSeries;

public sealed class TournamentSeriesTests
{
    [Fact]
    public void Create_Should_Create_TournamentSeries()
    {
        // Arrange
        var organizationId = OrganizationId.New();
        const string name = "Monthly Championship";

        // Act
        var series = TournamentSeriesEntity.Create(
            organizationId,
            name);

        // Assert
        Assert.NotEqual(default, series.Id);
        Assert.Equal(name, series.Name);
        Assert.Equal(
            organizationId,
            series.OrganizationId);
    }

    [Fact]
    public void Create_Should_Trim_Name()
    {
        // Arrange
        var organizationId = OrganizationId.New();

        // Act
        var series = TournamentSeriesEntity.Create(
            organizationId,
            "  Monthly Championship  ");

        // Assert
        Assert.Equal(
            "Monthly Championship",
            series.Name);
        Assert.Equal(
            organizationId,
            series.OrganizationId);
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Empty()
    {
        // Arrange
        var organizationId = OrganizationId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            TournamentSeriesEntity.Create(
                organizationId,
                "   "));
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        // Arrange
        var series = CreateTournamentSeries();

        // Act
        series.Rename("  Summer Open Series  ");

        // Assert
        Assert.Equal(
            "Summer Open Series",
            series.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        // Arrange
        var series = CreateTournamentSeries();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            series.Rename("   "));
    }

    private static TournamentSeriesEntity CreateTournamentSeries()
        => TournamentSeriesEntity.Create(
            OrganizationId.New(),
            "Test Tournament Series");
}
