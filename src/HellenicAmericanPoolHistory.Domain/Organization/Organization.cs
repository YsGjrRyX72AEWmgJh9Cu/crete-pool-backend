using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Organization;

/// <summary>
/// Represents an organization responsible for managing tournaments.
/// </summary>
public sealed class Organization : Entity<OrganizationId>
{
    private Organization(OrganizationId id)
        : base(id)
    {
    }

    public string Name { get; private set; } = string.Empty;

    public static Organization Create(string name)
    {
        var organization = new Organization(
            OrganizationId.New());

        organization.Rename(name);

        return organization;
    }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
    }
}
