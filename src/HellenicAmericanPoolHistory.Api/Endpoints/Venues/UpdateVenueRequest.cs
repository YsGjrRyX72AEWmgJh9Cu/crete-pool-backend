namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

/// <summary>
/// Request body for updating a venue.
/// </summary>
public sealed record UpdateVenueRequest(
    string Name,
    string City,
    string Address);