using HellenicAmericanPoolHistory.Domain.Organization;

namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;

/// <summary>
/// Represents the request to retrieve tournament series for an organization.
/// </summary>
public sealed record GetTournamentSeriesByOrganizationQuery(
    OrganizationId OrganizationId);
