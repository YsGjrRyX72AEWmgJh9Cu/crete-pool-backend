namespace HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;

/// <summary>
/// Represents the organization returned by the Get Organization feature.
/// </summary>
/// <param name="Id">The organization identifier.</param>
/// <param name="Name">The organization name.</param>
public sealed record GetOrganizationResponse(
    Guid Id,
    string Name);
