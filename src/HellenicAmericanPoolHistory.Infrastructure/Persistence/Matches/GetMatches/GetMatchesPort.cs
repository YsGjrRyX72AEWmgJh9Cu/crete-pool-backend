using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.GetMatches;

/// <summary>
/// Retrieves matches from the database.
/// </summary>
public sealed class GetMatchesPort : IGetMatchesPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetMatchesPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<GetMatchesResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var matches = await _dbContext.Matches
            .AsNoTracking()
            .Include(x => x.Tournament)
            .Include(x => x.Participant1)
                .ThenInclude(x => x.Player)
            .Include(x => x.Participant2)
                .ThenInclude(x => x.Player)
            .Include(x => x.Winner)
                .ThenInclude(x => x.Player)
            .OrderBy(x => x.Tournament.Name)
            .ThenBy(x => x.Tournament.StartDate)
            .ToListAsync(cancellationToken);

        return matches
            .Select(match => new GetMatchesResponse(
                match.Id.Value,
                match.TournamentId.Value,
                match.Tournament.Name,
                match.Participant1Id.Value,
                match.Participant1.Player.FullName,
                match.Participant2Id.Value,
                match.Participant2.Player.FullName,
                match.WinnerParticipationId?.Value,
                match.Winner?.Player.FullName,
                match.Participant1Score,
                match.Participant2Score))
            .ToList();
    }
}
