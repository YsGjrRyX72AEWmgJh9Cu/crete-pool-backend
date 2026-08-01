using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.GetVenue;

/// <summary>
/// Retrieves venues from the database.
/// </summary>
public sealed class GetVenuePort : IGetVenuePort
{
    private readonly ApplicationDbContext _dbContext;

    public GetVenuePort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<GetVenueResponse?> GetByIdAsync(
        Guid venueId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Venues
            .AsNoTracking()
            .Where(v => v.Id == new Domain.Venue.VenueId(venueId))
            .Select(v => new GetVenueResponse(
                v.Id.Value,
                v.Name,
                v.Location.Country,
                v.Location.City,
                v.Location.Address))
            .SingleOrDefaultAsync(cancellationToken);
    }
}