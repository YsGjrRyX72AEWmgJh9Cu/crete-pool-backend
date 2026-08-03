using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;

/// <summary>
/// Updates an existing venue.
/// </summary>
public interface IUpdateVenuePort
{
    Task UpdateAsync(
        VenueId venueId,
        VenueData data,
        CancellationToken cancellationToken);
}