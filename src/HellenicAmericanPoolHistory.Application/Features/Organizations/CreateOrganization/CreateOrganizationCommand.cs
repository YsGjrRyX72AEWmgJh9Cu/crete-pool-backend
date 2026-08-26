namespace HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;

/// <summary>
/// Represents the request to create an organization.
/// </summary>
public sealed record CreateOrganizationCommand(
    string Name);
