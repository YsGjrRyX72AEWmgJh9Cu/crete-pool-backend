namespace HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;

/// <summary>
/// Represents the data required to update an existing venue.
/// </summary>
public sealed record UpdateVenueCommand(
    Guid VenueId,
    string Name,
    string City,
    string Address);