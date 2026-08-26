using HellenicAmericanPoolHistory.Domain.Organization;

namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;

/// <summary>
/// Defines the persistence contract for retrieving tournament series
/// belonging to an organization.
/// </summary>
public interface IGetTournamentSeriesByOrganizationPort
{
    Task<IReadOnlyList<GetTournamentSeriesByOrganizationResponse>> GetAllAsync(
        OrganizationId organizationId,
        CancellationToken cancellationToken);
}
