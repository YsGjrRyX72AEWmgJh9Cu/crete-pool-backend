using Microsoft.EntityFrameworkCore;
using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.DeletePlayer;

public sealed class DeletePlayerPort : IDeletePlayerPort
{
    private readonly ApplicationDbContext _context;

    public DeletePlayerPort(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(
        DeletePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(
                player => player.Id == command.Id,
                cancellationToken);

        if (player is null)
        {
            throw new NotFoundException("Player not found.");
        }

        _context.Players.Remove(player);

        await _context.SaveChangesAsync(cancellationToken);
    }
}