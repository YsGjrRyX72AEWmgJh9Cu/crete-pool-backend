namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;

/// <summary>
/// Represents the request to retrieve an organization.
/// </summary>
public sealed record GetOrganizationQuery(Guid OrganizationId);
