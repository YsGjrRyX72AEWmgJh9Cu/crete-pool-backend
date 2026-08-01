using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;

/// <summary>
/// Persists newly created venues.
/// </summary>
public interface ICreateVenuePort
{
    Task SaveAsync(
        Venue venue,
        CancellationToken cancellationToken);
}