namespace HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;

/// <summary>
/// Represents a venue returned by the Get Venues feature.
/// </summary>
public sealed record GetVenuesResponse(
    Guid Id,
    string Name,
    string Country,
    string City,
    string? Address);