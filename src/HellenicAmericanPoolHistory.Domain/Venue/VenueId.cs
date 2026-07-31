using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Venue;

/// <summary>
/// Strongly typed identifier for a venue.
/// </summary>
public readonly record struct VenueId(Guid Value)
    : IStronglyTypedId<Guid>
{
    /// <summary>
    /// Creates a new unique venue identifier.
    /// </summary>
    /// <returns>A new <see cref="VenueId"/>.</returns>
    public static VenueId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}