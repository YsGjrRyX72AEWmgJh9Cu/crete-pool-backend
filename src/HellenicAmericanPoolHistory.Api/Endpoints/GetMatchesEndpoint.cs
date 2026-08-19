using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

/// <summary>
/// Maps the endpoint for retrieving matches.
/// </summary>
public static class GetMatchesEndpoint
{
    /// <summary>
    /// Maps the Get Matches endpoint.
    /// </summary>
    public static IEndpointRouteBuilder MapGetMatchesEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/matches",
            async Task<Ok<IReadOnlyCollection<GetMatchesResponse>>> (
                GetMatchesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var matches = await handler.HandleAsync(
                    new GetMatchesQuery(),
                    cancellationToken);

                return TypedResults.Ok(matches);
            })
            .WithName("GetMatches")
            .WithSummary("Gets all matches.")
            .WithDescription("Gets all matches.")
            .Produces<IReadOnlyCollection<GetMatchesResponse>>(
                StatusCodes.Status200OK);

        return endpoints;
    }
}
