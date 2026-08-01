namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;

/// <summary>
/// Represents the request to retrieve a venue.
/// </summary>
public sealed record GetVenueQuery(Guid VenueId);