using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Organization;

/// <summary>
/// Strongly typed identifier for an organization.
/// </summary>
public readonly record struct OrganizationId(Guid Value)
    : IStronglyTypedId<Guid>
{
    /// <summary>
    /// Creates a new unique organization identifier.
    /// </summary>
    public static OrganizationId New()
        => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}
