namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;

/// <summary>
/// Handles requests to retrieve an organization.
/// </summary>
public sealed class GetOrganizationHandler(
    IGetOrganizationPort port)
{
    public Task<GetOrganizationResponse?> HandleAsync(
        GetOrganizationQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetByIdAsync(
            query.OrganizationId,
            cancellationToken);
    }
}
