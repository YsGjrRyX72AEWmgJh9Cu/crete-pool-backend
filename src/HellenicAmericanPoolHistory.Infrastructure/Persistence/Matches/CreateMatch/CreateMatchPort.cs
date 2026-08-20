using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.CreateMatch;

/// <summary>
/// Persists newly created matches.
/// </summary>
public sealed class CreateMatchPort : ICreateMatchPort
{
    private readonly ApplicationDbContext _dbContext;

    public CreateMatchPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<MatchId> CreateAsync(
        Match match,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(match);

        var tournamentExists = await _dbContext.Tournaments
            .AnyAsync(
                tournament => tournament.Id == match.TournamentId,
                cancellationToken);

        if (!tournamentExists)
        {
            throw new NotFoundException("Tournament not found.");
        }

        var participations = await _dbContext.Participations
            .Where(participation =>
                participation.Id == match.Participant1Id ||
                participation.Id == match.Participant2Id ||
                (match.WinnerParticipationId.HasValue &&
                 participation.Id == match.WinnerParticipationId.Value))
            .ToListAsync(cancellationToken);

        if (participations.Count < 2)
        {
            throw new NotFoundException(
                "One or more match participations were not found.");
        }

        var participant1 = participations
            .Single(participation =>
                participation.Id == match.Participant1Id);

        var participant2 = participations
            .Single(participation =>
                participation.Id == match.Participant2Id);

        if (participant1.TournamentId != match.TournamentId)
        {
            throw new ConflictException(
                "Participant 1 does not belong to the specified tournament.");
        }

        if (participant2.TournamentId != match.TournamentId)
        {
            throw new ConflictException(
                "Participant 2 does not belong to the specified tournament.");
        }

        if (match.WinnerParticipationId.HasValue)
        {
            var winner = participations
                .Single(participation =>
                    participation.Id == match.WinnerParticipationId.Value);

            if (winner.TournamentId != match.TournamentId)
            {
                throw new ConflictException(
                    "Winner does not belong to the specified tournament.");
            }
        }

        _dbContext.Matches.Add(match);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return match.Id;
    }
}
