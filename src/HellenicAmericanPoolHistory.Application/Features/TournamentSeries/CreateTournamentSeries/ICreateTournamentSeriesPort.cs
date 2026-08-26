using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;

/// <summary>
/// Defines the persistence contract for creating tournament series.
/// </summary>
public interface ICreateTournamentSeriesPort
{
    Task SaveAsync(
        TournamentSeriesEntity tournamentSeries,
        CancellationToken cancellationToken);
}
