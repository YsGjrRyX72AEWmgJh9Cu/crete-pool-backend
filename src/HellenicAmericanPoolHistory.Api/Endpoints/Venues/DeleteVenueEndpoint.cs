using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

public static class DeleteVenueEndpoint
{
    public static IEndpointRouteBuilder MapDeleteVenueEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/venues/{id:guid}",
            async Task<NoContent>(
                Guid id,
                DeleteVenueHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    new DeleteVenueCommand(id),
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("DeleteVenue")
            .WithSummary("Deletes an existing venue.")
            .WithDescription("Deletes an existing venue.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}