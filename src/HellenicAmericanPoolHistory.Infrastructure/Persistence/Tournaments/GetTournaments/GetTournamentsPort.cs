using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournaments;

/// <summary>
/// Retrieves tournaments from the database.
/// </summary>
public sealed class GetTournamentsPort : IGetTournamentsPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetTournamentsPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GetTournamentsResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Tournaments
            .AsNoTracking()
            .OrderBy(t => t.StartDate)
            .ThenBy(t => t.Name)
            .Select(t => new GetTournamentsResponse(
                t.Id.Value,
                t.Name,
                t.TournamentType,
                t.BracketType,
                t.GameSet,
                t.TournamentStatus,
                t.StartDate,
                t.EndDate,
                t.VenueId.Value))
            .ToListAsync(cancellationToken);
    }
}