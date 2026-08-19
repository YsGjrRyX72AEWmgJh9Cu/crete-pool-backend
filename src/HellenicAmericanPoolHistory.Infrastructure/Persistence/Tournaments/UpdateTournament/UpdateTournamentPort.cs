using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.UpdateTournament;

/// <summary>
/// Updates existing tournaments.
/// </summary>
public sealed class UpdateTournamentPort : IUpdateTournamentPort
{
    private readonly ApplicationDbContext _context;

    public UpdateTournamentPort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task UpdateAsync(
        TournamentId tournamentId,
        TournamentData data,
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

        tournament.Edit(data);

        await _context.SaveChangesAsync(cancellationToken);
    }
}