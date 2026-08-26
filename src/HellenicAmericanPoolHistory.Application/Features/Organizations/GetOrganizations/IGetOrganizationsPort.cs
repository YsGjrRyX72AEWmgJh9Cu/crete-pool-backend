namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;

/// <summary>
/// Defines the persistence contract for retrieving organizations.
/// </summary>
public interface IGetOrganizationsPort
{
    Task<IReadOnlyList<GetOrganizationsResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}
