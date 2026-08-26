using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Organization;

namespace HellenicAmericanPoolHistory.Domain.TournamentSeries;

/// <summary>
/// Represents a recurring collection of tournaments.
/// </summary>
public sealed class TournamentSeries : Entity<TournamentSeriesId>
{
    private TournamentSeries(
        TournamentSeriesId id,
        OrganizationId organizationId)
        : base(id)
    {
        OrganizationId = organizationId;
    }

    public OrganizationId OrganizationId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public static TournamentSeries Create(
        OrganizationId organizationId,
        string name)
    {
        var series = new TournamentSeries(
            TournamentSeriesId.New(),
            organizationId);

        series.Rename(name);

        return series;
    }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
    }
}
