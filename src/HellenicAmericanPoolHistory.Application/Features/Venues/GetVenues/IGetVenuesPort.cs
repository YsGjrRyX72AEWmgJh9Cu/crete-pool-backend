namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;

/// <summary>
/// Retrieves all venues.
/// </summary>
public interface IGetVenuesPort
{
    Task<IReadOnlyList<GetVenuesResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}