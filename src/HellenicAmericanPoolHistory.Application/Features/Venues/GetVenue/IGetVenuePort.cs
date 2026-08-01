namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;

/// <summary>
/// Retrieves venues.
/// </summary>
public interface IGetVenuePort
{
    Task<GetVenueResponse?> GetByIdAsync(
        Guid venueId,
        CancellationToken cancellationToken);
}