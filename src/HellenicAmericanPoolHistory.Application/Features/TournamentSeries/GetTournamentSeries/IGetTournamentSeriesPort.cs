namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;

/// <summary>
/// Defines the persistence contract for retrieving tournament series.
/// </summary>
public interface IGetTournamentSeriesPort
{
    Task<IReadOnlyList<GetTournamentSeriesResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}
