using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.CreateVenue;

/// <summary>
/// Persists newly created venues.
/// </summary>
public sealed class CreateVenuePort : ICreateVenuePort
{
    private readonly ApplicationDbContext _dbContext;

    public CreateVenuePort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task SaveAsync(
        Venue venue,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(venue);

        await _dbContext.Venues.AddAsync(
            venue,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}