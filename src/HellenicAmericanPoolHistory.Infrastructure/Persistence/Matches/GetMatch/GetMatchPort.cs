using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.GetMatch;

/// <summary>
/// Retrieves a match from the database.
/// </summary>
public sealed class GetMatchPort : IGetMatchPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetMatchPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<GetMatchResponse?> GetByIdAsync(
        MatchId matchId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Matches
            .AsNoTracking()
            .Where(match => match.Id == matchId)
            .Select(match => new GetMatchResponse(
                match.Id.Value,
                match.TournamentId.Value,
                match.Tournament.Name,
                match.Participant1Id.Value,
                match.Participant1.Player.FullName,
                match.Participant2Id.Value,
                match.Participant2.Player.FullName,
                match.WinnerParticipationId != null
                    ? match.WinnerParticipationId.Value.Value
                    : null,
                match.WinnerParticipationId != null
                    ? match.Winner.Player.FullName
                    : null,
                match.Participant1Score,
                match.Participant2Score))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
