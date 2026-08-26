using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.TournamentSeries.GetTournamentSeries;

/// <summary>
/// Retrieves tournament series from persistence.
/// </summary>
public sealed class GetTournamentSeriesPort : IGetTournamentSeriesPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetTournamentSeriesPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GetTournamentSeriesResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.TournamentSeries
            .AsNoTracking()
            .OrderBy(series => series.Name)
            .Select(
                series => new GetTournamentSeriesResponse(
                    series.Id.Value,
                    series.Name,
                    series.OrganizationId.Value))
            .ToListAsync(cancellationToken);
    }
}
