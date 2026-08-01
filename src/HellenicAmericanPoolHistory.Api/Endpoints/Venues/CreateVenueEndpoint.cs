using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Venues;

public static class CreateVenueEndpoint
{
    public static IEndpointRouteBuilder MapCreateVenueEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/venues",
            async Task<Created<CreateVenueResponse>> (
                CreateVenueCommand command,
                CreateVenueHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(command, cancellationToken);

                return TypedResults.Created(
                    $"/venues/{response.VenueId}",
                    response);
            })
            .AddEndpointFilter<ValidationFilter<CreateVenueCommand>>()
            .WithName("CreateVenue")
            .WithSummary("Creates a new venue.")
            .WithDescription("Creates a new venue and returns its identifier.")
            .Produces<CreateVenueResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}