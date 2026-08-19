using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

public static class DeleteMatchEndpoint
{
    public static IEndpointRouteBuilder MapDeleteMatchEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/matches/{id:guid}",
            async Task<NoContent>(
                Guid id,
                DeleteMatchHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteMatchCommand(
                    new MatchId(id));

                await handler.Handle(
                    command,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("DeleteMatch")
            .WithSummary("Deletes an existing match.")
            .WithDescription("Deletes an existing match.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
