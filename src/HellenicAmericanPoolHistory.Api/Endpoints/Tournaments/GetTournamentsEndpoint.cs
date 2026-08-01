using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class GetTournamentsEndpoint
{
    public static IEndpointRouteBuilder MapGetTournamentsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/tournaments",
            async Task<Ok<IReadOnlyList<GetTournamentsResponse>>>(
                GetTournamentsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(cancellationToken);

                return TypedResults.Ok(response);
            })
            .WithName("GetTournaments")
            .WithSummary("Gets all tournaments.")
            .WithDescription("Returns all tournaments.")
            .Produces<IReadOnlyList<GetTournamentsResponse>>(StatusCodes.Status200OK);

        return endpoints;
    }
}