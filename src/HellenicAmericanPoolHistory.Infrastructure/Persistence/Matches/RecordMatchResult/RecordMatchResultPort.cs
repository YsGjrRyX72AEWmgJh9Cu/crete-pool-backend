using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.RecordMatchResult;

/// <summary>
/// Persists match results.
/// </summary>
public sealed class RecordMatchResultPort : IRecordMatchResultPort
{
    private readonly ApplicationDbContext _dbContext;

    public RecordMatchResultPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task RecordAsync(
        MatchId matchId,
        ParticipationId winnerParticipationId,
        int participant1Score,
        int participant2Score,
        CancellationToken cancellationToken)
    {
        var match = await _dbContext.Matches
            .Include(match => match.Tournament)
            .FirstOrDefaultAsync(
                match => match.Id == matchId,
                cancellationToken);

        if (match is null)
        {
            throw new NotFoundException("Match not found.");
        }

        if (match.Tournament.TournamentStatus !=
            TournamentStatus.InProgress)
        {
            throw new ConflictException(
                "Match result can only be recorded while the tournament is in progress.");
        }

        try
        {
            match.RecordResult(
                winnerParticipationId,
                participant1Score,
                participant2Score);
        }
        catch (InvalidOperationException exception)
        {
            throw new ConflictException(exception.Message);
        }
        catch (ArgumentException exception)
        {
            throw new ConflictException(exception.Message);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
