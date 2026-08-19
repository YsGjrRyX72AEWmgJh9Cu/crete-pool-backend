using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

/// <summary>
/// Maps the endpoint for retrieving a participation.
/// </summary>
public static class GetParticipationEndpoint
{
    /// <summary>
    /// Maps the Get Participation endpoint.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapGetParticipationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/participations/{id:guid}",
            async Task<Results<Ok<GetParticipationResponse>, NotFound>> (
                Guid id,
                GetParticipationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var participation = await handler.HandleAsync(
                    new GetParticipationQuery(new ParticipationId(id)),
                    cancellationToken);

                return participation is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(participation);
            })
            .WithName("GetParticipation")
            .WithSummary("Gets a participation by identifier.")
            .WithDescription("Gets a participation by its identifier.")
            .Produces<GetParticipationResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}