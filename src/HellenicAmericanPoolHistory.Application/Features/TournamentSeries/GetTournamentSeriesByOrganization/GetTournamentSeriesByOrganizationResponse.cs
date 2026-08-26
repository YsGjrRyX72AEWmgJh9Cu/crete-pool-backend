namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;

/// <summary>
/// Represents a tournament series returned for an organization.
/// </summary>
public sealed record GetTournamentSeriesByOrganizationResponse(
    Guid Id,
    string Name,
    Guid OrganizationId);
