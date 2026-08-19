using Microsoft.EntityFrameworkCore;
using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.DeleteParticipation;

public sealed class DeleteParticipationPort : IDeleteParticipationPort
{
    private readonly ApplicationDbContext _context;

    public DeleteParticipationPort(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(
        DeleteParticipationCommand command,
        CancellationToken cancellationToken)
    {
        var participation = await _context.Participations
            .FirstOrDefaultAsync(
                participation => participation.Id == command.Id,
                cancellationToken);

        if (participation is null)
        {
            throw new NotFoundException("Participation not found.");
        }

        _context.Participations.Remove(participation);

        await _context.SaveChangesAsync(cancellationToken);
    }
}