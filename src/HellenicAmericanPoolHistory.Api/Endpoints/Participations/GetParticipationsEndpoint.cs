using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

/// <summary>
/// Maps the endpoint for retrieving participations.
/// </summary>
public static class GetParticipationsEndpoint
{
    /// <summary>
    /// Maps the Get Participations endpoint.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapGetParticipationsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/participations",
            async Task<Ok<IReadOnlyCollection<GetParticipationsResponse>>> (
                GetParticipationsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var participations = await handler.HandleAsync(
                    new GetParticipationsQuery(),
                    cancellationToken);

                return TypedResults.Ok(participations);
            })
            .WithName("GetParticipations")
            .WithSummary("Gets all participations.")
            .WithDescription("Gets all participations.")
            .Produces<IReadOnlyCollection<GetParticipationsResponse>>(
                StatusCodes.Status200OK);

        return endpoints;
    }
}