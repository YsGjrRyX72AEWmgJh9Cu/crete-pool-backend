using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

public static class UpdateVenueEndpoint
{
    public static IEndpointRouteBuilder MapUpdateVenueEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "/venues/{id:guid}",
            async Task<NoContent>(
                Guid id,
                UpdateVenueRequest request,
                UpdateVenueHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateVenueCommand(
                    id,
                    request.Name,
                    request.City,
                    request.Address);

                await handler.Handle(
                    command,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateVenueRequest>>()
            .WithName("UpdateVenue")
            .WithSummary("Updates an existing venue.")
            .WithDescription("Updates an existing venue.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}