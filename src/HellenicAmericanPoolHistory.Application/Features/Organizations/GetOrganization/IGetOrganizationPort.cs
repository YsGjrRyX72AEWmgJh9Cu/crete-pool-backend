namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;

/// <summary>
/// Retrieves organizations.
/// </summary>
public interface IGetOrganizationPort
{
    Task<GetOrganizationResponse?> GetByIdAsync(
        Guid organizationId,
        CancellationToken cancellationToken);
}
