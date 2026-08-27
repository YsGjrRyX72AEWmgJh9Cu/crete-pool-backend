using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Organizations.GetOrganization;

public static class GetOrganizationEndpoint
{
    public static IEndpointRouteBuilder MapGetOrganizationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/organizations/{id:guid}",
            async Task<Ok<GetOrganizationResponse>> (
                Guid id,
                GetOrganizationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(
                        new GetOrganizationQuery(id),
                        cancellationToken);

                if (response is null)
                {
                    throw new NotFoundException(
                        $"Organization '{id}' was not found.");
                }

                return TypedResults.Ok(response);
            })
            .WithName("GetOrganization")
            .WithSummary("Gets an organization by identifier.")
            .WithDescription(
                "Returns an organization by its identifier.")
            .Produces<GetOrganizationResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status404NotFound);

        return endpoints;
    }
}
