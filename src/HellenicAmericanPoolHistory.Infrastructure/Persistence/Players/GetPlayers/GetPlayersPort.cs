using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.GetPlayers;

public sealed class GetPlayersPort : IGetPlayersPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetPlayersPort(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GetPlayersResponse>> GetAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Players
            .AsNoTracking()
            .Select(player => new GetPlayersResponse(
                player.Id.Value,
                player.FirstName,
                player.LastName,
                player.CountryOfOrigin.Value,
                player.BirthDate))
            .ToListAsync(cancellationToken);
    }
}