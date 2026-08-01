namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;

/// <summary>
/// Handles requests to retrieve all venues.
/// </summary>
public sealed class GetVenuesHandler(IGetVenuesPort port)
{
    public Task<IReadOnlyList<GetVenuesResponse>> HandleAsync(
        CancellationToken cancellationToken)
    {
        return port.GetAllAsync(cancellationToken);
    }
}