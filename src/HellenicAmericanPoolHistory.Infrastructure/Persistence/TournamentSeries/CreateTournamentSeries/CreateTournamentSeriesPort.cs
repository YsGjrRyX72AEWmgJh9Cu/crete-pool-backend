using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.TournamentSeries.CreateTournamentSeries;

/// <summary>
/// Persists newly created tournament series.
/// </summary>
public sealed class CreateTournamentSeriesPort
    : ICreateTournamentSeriesPort
{
    private readonly ApplicationDbContext _dbContext;

    public CreateTournamentSeriesPort(
        ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task SaveAsync(
        TournamentSeriesEntity tournamentSeries,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tournamentSeries);

        await _dbContext.TournamentSeries.AddAsync(
            tournamentSeries,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}
