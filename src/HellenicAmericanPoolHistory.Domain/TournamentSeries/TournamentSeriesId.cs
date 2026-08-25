using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.TournamentSeries;

/// <summary>
/// Strongly typed identifier for a tournament series.
/// </summary>
public readonly record struct TournamentSeriesId(Guid Value)
    : IStronglyTypedId<Guid>
{
    /// <summary>
    /// Creates a new unique tournament series identifier.
    /// </summary>
    public static TournamentSeriesId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}
