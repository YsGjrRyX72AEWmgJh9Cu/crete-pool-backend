using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.GetVenues;

/// <summary>
/// Retrieves venues from the database.
/// </summary>
public sealed class GetVenuesPort : IGetVenuesPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetVenuesPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GetVenuesResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Venues
            .AsNoTracking()
            .OrderBy(v => v.Name)
            .Select(v => new GetVenuesResponse(
                v.Id.Value,
                v.Name,
                v.Location.Country,
                v.Location.City,
                v.Location.Address))
            .ToListAsync(cancellationToken);
    }
}