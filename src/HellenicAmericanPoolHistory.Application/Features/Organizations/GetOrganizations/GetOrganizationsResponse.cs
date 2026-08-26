namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;

/// <summary>
/// Represents an organization returned by the Get Organizations feature.
/// </summary>
public sealed record GetOrganizationsResponse(
    Guid Id,
    string Name);
