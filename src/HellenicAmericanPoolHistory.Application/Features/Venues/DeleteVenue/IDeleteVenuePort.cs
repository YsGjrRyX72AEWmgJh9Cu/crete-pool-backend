using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;

/// <summary>
/// Deletes venues from persistence.
/// </summary>
public interface IDeleteVenuePort
{
    Task DeleteAsync(
        VenueId venueId,
        CancellationToken cancellationToken);
}