using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;
using HellenicAmericanPoolHistory.Domain.Tournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.DeleteTournament;

/// <summary>
/// Deletes tournaments from the database.
/// </summary>
public sealed class DeleteTournamentPort : IDeleteTournamentPort
{
    private readonly ApplicationDbContext _context;

    public DeleteTournamentPort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task DeleteAsync(
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

        _context.Tournaments.Remove(tournament);

        await _context.SaveChangesAsync(cancellationToken);
    }
}