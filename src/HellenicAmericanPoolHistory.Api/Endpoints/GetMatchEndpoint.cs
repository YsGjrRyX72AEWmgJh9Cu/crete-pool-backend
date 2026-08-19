using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

/// <summary>
/// Maps the endpoint for retrieving a match.
/// </summary>
public static class GetMatchEndpoint
{
    /// <summary>
    /// Maps the Get Match endpoint.
    /// </summary>
    public static IEndpointRouteBuilder MapGetMatchEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/matches/{id:guid}",
            async Task<Results<Ok<GetMatchResponse>, NotFound>> (
                Guid id,
                GetMatchHandler handler,
                CancellationToken cancellationToken) =>
            {
                var match = await handler.HandleAsync(
                    new GetMatchQuery(new MatchId(id)),
                    cancellationToken);

                return match is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(match);
            })
            .WithName("GetMatch")
            .WithSummary("Gets a match by identifier.")
            .WithDescription("Gets a match by its identifier.")
            .Produces<GetMatchResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
