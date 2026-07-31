using Microsoft.EntityFrameworkCore;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.UpdatePlayer;

public sealed class UpdatePlayerPort : IUpdatePlayerPort
{
    private readonly ApplicationDbContext _context;

    public UpdatePlayerPort(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateAsync(
        UpdatePlayerCommand command,
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

        player.Update(
            command.FirstName,
            command.LastName,
            new Country(command.CountryOfOrigin),
            command.BirthDate);

        await _context.SaveChangesAsync(cancellationToken);
    }
}