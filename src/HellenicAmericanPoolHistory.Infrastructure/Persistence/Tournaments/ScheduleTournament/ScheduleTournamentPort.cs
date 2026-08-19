using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.ScheduleTournament;

/// <summary>
/// Schedules tournaments in the database.
/// </summary>
public sealed class ScheduleTournamentPort : IScheduleTournamentPort
{
    private readonly ApplicationDbContext _context;

    public ScheduleTournamentPort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task ScheduleAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken)
    {
        var tournament = await _context.Tournaments
            .FirstOrDefaultAsync(
                tournament => tournament.Id == tournamentId,
                cancellationToken);

        if (tournament is null)
        {
            throw new NotFoundException("Tournament not found.");
        }

        try
        {
            tournament.Schedule();
        }
        catch (InvalidOperationException exception)
        {
            throw new ConflictException(exception.Message);
        } 

        await _context.SaveChangesAsync(cancellationToken);
    }
}
