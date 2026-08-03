namespace HellenicAmericanPoolHistory.Domain.Venue;

/// <summary>
/// Represents the data required to create or update a venue.
/// </summary>
public sealed record VenueData(
    string Name,
    string City,
    string Address);