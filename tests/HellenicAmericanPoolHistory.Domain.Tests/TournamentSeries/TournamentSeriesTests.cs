using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Domain.Tests.TournamentSeries;

public sealed class TournamentSeriesTests
{
    [Fact]
    public void Create_Should_Create_TournamentSeries()
    {
        const string name = "Monthly Championship";

        var series = TournamentSeriesEntity.Create(name);

        Assert.NotEqual(default, series.Id);
        Assert.Equal(name, series.Name);
    }

    [Fact]
    public void Create_Should_Trim_Name()
    {
        var series = TournamentSeriesEntity.Create(
            "  Monthly Championship  ");

        Assert.Equal(
            "Monthly Championship",
            series.Name);
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            TournamentSeriesEntity.Create("   "));
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        var series = CreateTournamentSeries();

        series.Rename("  Summer Open Series  ");

        Assert.Equal(
            "Summer Open Series",
            series.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        var series = CreateTournamentSeries();

        Assert.Throws<ArgumentException>(() =>
            series.Rename("   "));
    }

    private static TournamentSeriesEntity CreateTournamentSeries()
        => TournamentSeriesEntity.Create(
            "Test Tournament Series");
}
