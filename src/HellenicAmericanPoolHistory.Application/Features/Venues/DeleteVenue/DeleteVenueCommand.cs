namespace HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;

/// <summary>
/// Deletes an existing venue.
/// </summary>
public sealed record DeleteVenueCommand(Guid VenueId);