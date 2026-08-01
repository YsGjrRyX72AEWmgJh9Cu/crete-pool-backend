namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;

/// <summary>
/// Represents the venue returned by the Get Venue feature.
/// </summary>
/// <param name="Id">The venue identifier.</param>
/// <param name="Name">The venue name.</param>
/// <param name="Country">The country.</param>
/// <param name="City">The city.</param>
/// <param name="Address">The address.</param>
public sealed record GetVenueResponse(
    Guid Id,
    string Name,
    string Country,
    string City,
    string? Address);