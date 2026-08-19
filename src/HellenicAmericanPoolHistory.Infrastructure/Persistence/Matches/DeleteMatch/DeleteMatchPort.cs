using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.DeleteMatch;

public sealed class DeleteMatchPort : IDeleteMatchPort
{
    private readonly ApplicationDbContext _context;

    public DeleteMatchPort(
        ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task DeleteAsync(
        DeleteMatchCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var match = await _context.Matches
            .FirstOrDefaultAsync(
                match => match.Id == command.Id,
                cancellationToken);

        if (match is null)
        {
            throw new NotFoundException("Match not found.");
        }

        _context.Matches.Remove(match);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
