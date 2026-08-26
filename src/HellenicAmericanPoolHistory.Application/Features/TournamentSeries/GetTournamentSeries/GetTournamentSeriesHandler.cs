namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;

/// <summary>
/// Handles requests to retrieve all tournament series.
/// </summary>
public sealed class GetTournamentSeriesHandler(
    IGetTournamentSeriesPort port)
{
    public Task<IReadOnlyList<GetTournamentSeriesResponse>> HandleAsync(
        CancellationToken cancellationToken)
    {
        return port.GetAllAsync(cancellationToken);
    }
}
