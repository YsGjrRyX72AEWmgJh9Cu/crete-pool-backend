using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;

/// <summary>
/// Defines the persistence contract for creating organizations.
/// </summary>
public interface ICreateOrganizationPort
{
    Task SaveAsync(
        OrganizationEntity organization,
        CancellationToken cancellationToken);
}
