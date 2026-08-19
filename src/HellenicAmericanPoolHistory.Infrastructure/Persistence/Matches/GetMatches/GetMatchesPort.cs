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
        return await _dbContext.Matches
            .AsNoTracking()
            .OrderBy(match => match.Tournament.Name)
            .ThenBy(match => match.Tournament.StartDate)
            .Select(match => new GetMatchesResponse(
                match.Id.Value,
                match.TournamentId.Value,
                match.Tournament.Name,
                match.Participant1Id.Value,
                match.Participant1.Player.FullName,
                match.Participant2Id.Value,
                match.Participant2.Player.FullName,
                match.WinnerParticipationId.Value,
                match.Winner.Player.FullName,
                match.Participant1Score,
                match.Participant2Score))
            .ToListAsync(cancellationToken);
    }
}
