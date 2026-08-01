using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

public static class GetVenuesEndpoint
{
    public static IEndpointRouteBuilder MapGetVenuesEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/venues",
            async Task<Ok<IReadOnlyList<GetVenuesResponse>>>(
                GetVenuesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(cancellationToken);

                return TypedResults.Ok(response);
            })
            .WithName("GetVenues")
            .WithSummary("Gets all venues.")
            .WithDescription("Returns all venues.")
            .Produces<IReadOnlyList<GetVenuesResponse>>(StatusCodes.Status200OK);

        return endpoints;
    }
}