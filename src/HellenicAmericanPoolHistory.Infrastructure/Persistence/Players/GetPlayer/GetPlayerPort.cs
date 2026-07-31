using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.GetPlayer;

/// <summary>
/// Retrieves players from the database.
/// </summary>
public sealed class GetPlayerPort : IGetPlayerPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetPlayerPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public Task<GetPlayerResponse?> GetByIdAsync(
        PlayerId playerId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Players
            .AsNoTracking()
            .Where(player => player.Id == playerId)
            .Select(player => new GetPlayerResponse(
                player.Id.Value,
                player.FirstName,
                player.LastName,
                player.CountryOfOrigin.Value,
                player.BirthDate))
            .SingleOrDefaultAsync(cancellationToken);
    }
}