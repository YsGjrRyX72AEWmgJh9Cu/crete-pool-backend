using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.CreateParticipation;

/// <summary>
/// Persists newly created participations.
/// </summary>
public sealed class CreateParticipationPort : ICreateParticipationPort
{
    private readonly ApplicationDbContext _dbContext;

    public CreateParticipationPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<ParticipationId> CreateAsync(
        Participation participation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(participation);

        var playerExists = await _dbContext.Players
            .AnyAsync(
                player => player.Id == participation.PlayerId,
                cancellationToken);

        if (!playerExists)
        {
            throw new NotFoundException("Player not found.");
        }

        var tournament = await _dbContext.Tournaments
            .FirstOrDefaultAsync(
                tournament => tournament.Id == participation.TournamentId,
                cancellationToken);

        if (tournament is null)
        {
            throw new NotFoundException("Tournament not found.");
        }

        if (tournament.TournamentStatus is not TournamentStatus.Draft
            and not TournamentStatus.Scheduled)
        {
            throw new ConflictException(
                $"Participation cannot be created because tournament status is '{tournament.TournamentStatus}'.");
        }

        var alreadyExists = await _dbContext.Participations
            .AnyAsync(
                existing =>
                    existing.PlayerId == participation.PlayerId &&
                    existing.TournamentId == participation.TournamentId,
                cancellationToken);

        if (alreadyExists)
        {
            throw new ConflictException(
                "Player is already registered for this tournament.");
        }

        _dbContext.Participations.Add(participation);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return participation.Id;
    }
}