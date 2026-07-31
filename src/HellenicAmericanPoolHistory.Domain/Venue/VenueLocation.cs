namespace HellenicAmericanPoolHistory.Domain.Venue;

/// <summary>
/// Represents the physical location of a venue.
/// </summary>
public sealed record VenueLocation
{
    public VenueLocation(
        string country,
        string city,
        string? address = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);

        Country = country.Trim();
        City = city.Trim();
        Address = string.IsNullOrWhiteSpace(address)
            ? null
            : address.Trim();
    }

    public string Country { get; }

    public string City { get; }

    public string? Address { get; }
}