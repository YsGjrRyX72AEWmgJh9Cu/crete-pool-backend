using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Venue;

/// <summary>
/// Represents a venue where tournaments are held.
/// </summary>
public sealed class Venue : Entity<VenueId>
{
    private Venue(VenueId id)
        : base(id)
    {
    }

    public string Name { get; private set; } = string.Empty;

    public VenueLocation Location { get; private set; } = null!;

    public static Venue Create(
        string name,
        VenueLocation location)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(location);

        return new Venue(VenueId.New())
        {
            Name = name.Trim(),
            Location = location
        };
    }

    /// <summary>
    /// Updates the venue.
    /// </summary>
    public void Edit(VenueData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        Name = data.Name.Trim();

        Location = new VenueLocation(
            data.City,
            data.Address);
    }
}