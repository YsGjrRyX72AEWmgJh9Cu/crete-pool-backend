using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Players;

public static class DeletePlayerEndpoint
{
    public static IEndpointRouteBuilder MapDeletePlayerEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/players/{id:guid}",
            async Task<NoContent> (
                Guid id,
                DeletePlayerHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeletePlayerCommand(
                    new PlayerId(id));

                await handler.Handle(command, cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("DeletePlayer")
            .WithSummary("Deletes an existing player.")
            .WithDescription("Deletes an existing player.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}