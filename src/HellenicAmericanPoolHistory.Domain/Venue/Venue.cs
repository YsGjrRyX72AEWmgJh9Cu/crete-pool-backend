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

        var venue = new Venue(VenueId.New())
        {
            Name = name.Trim(),
            Location = location
        };

        return venue;
    }
}