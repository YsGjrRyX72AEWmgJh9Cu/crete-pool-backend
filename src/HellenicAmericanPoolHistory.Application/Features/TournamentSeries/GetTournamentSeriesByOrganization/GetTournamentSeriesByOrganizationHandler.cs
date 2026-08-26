using HellenicAmericanPoolHistory.Domain.Organization;

namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;

/// <summary>
/// Handles requests to retrieve tournament series for an organization.
/// </summary>
public sealed class GetTournamentSeriesByOrganizationHandler(
    IGetTournamentSeriesByOrganizationPort port)
{
    public Task<IReadOnlyList<GetTournamentSeriesByOrganizationResponse>> HandleAsync(
        OrganizationId organizationId,
        CancellationToken cancellationToken)
    {
        return port.GetAllAsync(
            organizationId,
            cancellationToken);
    }
}
