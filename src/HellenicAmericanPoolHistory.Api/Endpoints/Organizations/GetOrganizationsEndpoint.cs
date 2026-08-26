using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Organizations;

public static class GetOrganizationsEndpoint
{
    public static IEndpointRouteBuilder MapGetOrganizationsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/organizations",
            async Task<Ok<IReadOnlyList<GetOrganizationsResponse>>>(
                [FromServices] GetOrganizationsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(cancellationToken);

                return TypedResults.Ok(response);
            })
            .WithName("GetOrganizations")
            .WithSummary("Gets all organizations.")
            .WithDescription("Returns all organizations.")
            .Produces<IReadOnlyList<GetOrganizationsResponse>>(
                StatusCodes.Status200OK);

        return endpoints;
    }
}
