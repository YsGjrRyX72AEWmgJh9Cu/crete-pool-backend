using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;

/// <summary>
/// Represents the request to create a venue.
/// </summary>
public sealed record CreateVenueCommand(
    string Name,
    string Country,
    string City,
    string? Address);