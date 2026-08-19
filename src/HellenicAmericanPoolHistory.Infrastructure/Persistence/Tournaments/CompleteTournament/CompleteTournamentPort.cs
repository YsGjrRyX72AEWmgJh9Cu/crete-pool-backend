using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CompleteTournament;

/// <summary>
/// Completes tournaments in the database.
/// </summary>
public sealed class CompleteTournamentPort : ICompleteTournamentPort
{
    private readonly ApplicationDbContext _context;

    public CompleteTournamentPort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task CompleteAsync(
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
            tournament.Complete();
        }
        catch (InvalidOperationException exception)
        {
            throw new ConflictException(exception.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
