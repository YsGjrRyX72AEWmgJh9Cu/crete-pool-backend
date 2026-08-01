using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;
using HellenicAmericanPoolHistory.Domain.Tournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournament;

/// <summary>
/// Retrieves tournaments from the database.
/// </summary>
public sealed class GetTournamentPort : IGetTournamentPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetTournamentPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<GetTournamentResponse?> GetByIdAsync(
        Guid tournamentId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Tournaments
            .AsNoTracking()
            .Where(t => t.Id == new TournamentId(tournamentId))
            .Select(t => new GetTournamentResponse(
                t.Id.Value,
                t.Name,
                t.TournamentType,
                t.BracketType,
                t.GameSet,
                t.TournamentStatus,
                t.StartDate,
                t.EndDate,
                t.VenueId.Value))
            .SingleOrDefaultAsync(cancellationToken);
    }
}