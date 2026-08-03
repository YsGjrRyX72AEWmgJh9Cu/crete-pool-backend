using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Domain.Venue;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.DeleteVenue;

/// <summary>
/// Deletes venues from the database.
/// </summary>
public sealed class DeleteVenuePort : IDeleteVenuePort
{
    private readonly ApplicationDbContext _context;

    public DeleteVenuePort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task DeleteAsync(
        VenueId venueId,
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

        _context.Venues.Remove(venue);

        await _context.SaveChangesAsync(cancellationToken);
    }
}