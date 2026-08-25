using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.TournamentSeries;

/// <summary>
/// Represents a recurring collection of tournaments.
/// </summary>
public sealed class TournamentSeries : Entity<TournamentSeriesId>
{
    private TournamentSeries(TournamentSeriesId id)
        : base(id)
    {
    }

    public string Name { get; private set; } = string.Empty;

    public static TournamentSeries Create(string name)
    {
        var series = new TournamentSeries(
            TournamentSeriesId.New());

        series.Rename(name);

        return series;
    }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
    }
}
