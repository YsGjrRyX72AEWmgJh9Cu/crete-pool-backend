namespace HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;

/// <summary>
/// Represents the response after creating an organization.
/// </summary>
public sealed record CreateOrganizationResponse(
    Guid OrganizationId);
