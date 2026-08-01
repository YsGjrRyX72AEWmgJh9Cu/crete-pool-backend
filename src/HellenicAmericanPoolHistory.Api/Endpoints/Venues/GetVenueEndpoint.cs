using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

public static class GetVenueEndpoint
{
    public static IEndpointRouteBuilder MapGetVenueEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/venues/{id:guid}",
            async Task<Ok<GetVenueResponse>> (
                Guid id,
                GetVenueHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(
                        new GetVenueQuery(id),
                        cancellationToken);

                if (response is null)
                {
                    throw new NotFoundException(
                        $"Venue '{id}' was not found.");
                }

                return TypedResults.Ok(response);
            })
            .WithName("GetVenue")
            .WithSummary("Gets a venue by identifier.")
            .WithDescription("Returns a venue by its identifier.")
            .Produces<GetVenueResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}