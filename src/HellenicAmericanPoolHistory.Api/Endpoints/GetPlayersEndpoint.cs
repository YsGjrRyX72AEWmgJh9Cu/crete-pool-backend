using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Players;

public static class GetPlayersEndpoint
{
    public static IEndpointRouteBuilder MapGetPlayersEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/players",
            async Task<Ok<IReadOnlyList<GetPlayersResponse>>> (
                GetPlayersHandler handler,
                CancellationToken cancellationToken) =>
            {
                var players = await handler.HandleAsync(
                    new GetPlayersQuery(),
                    cancellationToken);

                return TypedResults.Ok(players);
            })
            .WithName("GetPlayers")
            .WithSummary("Gets all players.")
            .WithDescription("Gets all players.")
            .Produces<IReadOnlyList<GetPlayersResponse>>(StatusCodes.Status200OK);

        return endpoints;
    }
}