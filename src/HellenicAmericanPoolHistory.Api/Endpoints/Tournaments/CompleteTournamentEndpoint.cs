using HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class CompleteTournamentEndpoint
{
    public static IEndpointRouteBuilder MapCompleteTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments/{id:guid}/complete",
            async Task<NoContent>(
                Guid id,
                CompleteTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("CompleteTournament")
            .WithSummary("Completes a tournament.")
            .WithDescription(
                "Moves a tournament from InProgress to Completed.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
