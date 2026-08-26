namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;

/// <summary>
/// Handles requests to retrieve all organizations.
/// </summary>
public sealed class GetOrganizationsHandler(IGetOrganizationsPort port)
{
    public Task<IReadOnlyList<GetOrganizationsResponse>> HandleAsync(
        CancellationToken cancellationToken)
    {
        return port.GetAllAsync(cancellationToken);
    }
}
