using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Domain.Venue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.UpdateVenue;

/// <summary>
/// Updates existing venues.
/// </summary>
public sealed class UpdateVenuePort : IUpdateVenuePort
{
    private readonly ApplicationDbContext _context;

    public UpdateVenuePort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task UpdateAsync(
        VenueId venueId,
        VenueData data,
        CancellationToken cancellationToken)
    {
        var venue = await _context.Venues
            .FirstOrDefaultAsync(
                venue => venue.Id == venueId,
                cancellationToken);

        if (venue is null)
        {
            throw new NotFoundException("Venue not found.");
        }

        venue.Edit(data);

        await _context.SaveChangesAsync(cancellationToken);
    }
}