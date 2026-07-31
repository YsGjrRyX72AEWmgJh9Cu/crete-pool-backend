using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Players;

public static class GetPlayerEndpoint
{
    public static IEndpointRouteBuilder MapGetPlayerEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/players/{id:guid}",
            async Task<Results<Ok<GetPlayerResponse>, NotFound>> (
                Guid id,
                GetPlayerHandler handler,
                CancellationToken cancellationToken) =>
            {
                var player = await handler.HandleAsync(
                    new GetPlayerQuery(new PlayerId(id)),
                    cancellationToken);

                return player is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(player);
            })
            .WithName("GetPlayer")
            .WithSummary("Gets a player by identifier.")
            .WithDescription("Gets a player by its identifier.")
            .Produces<GetPlayerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}