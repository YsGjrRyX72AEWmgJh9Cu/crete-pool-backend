namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;

/// <summary>
/// Handles requests to retrieve a venue.
/// </summary>
public sealed class GetVenueHandler(IGetVenuePort port)
{
    public Task<GetVenueResponse?> HandleAsync(
        GetVenueQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetByIdAsync(
            query.VenueId,
            cancellationToken);
    }
}