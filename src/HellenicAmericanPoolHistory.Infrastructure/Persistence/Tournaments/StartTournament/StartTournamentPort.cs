using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.StartTournament;

/// <summary>
/// Starts tournaments in the database.
/// </summary>
public sealed class StartTournamentPort : IStartTournamentPort
{
    private readonly ApplicationDbContext _context;

    public StartTournamentPort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task StartAsync(
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
            tournament.Start();
        }
        catch (InvalidOperationException exception)
        {
            throw new ConflictException(exception.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
