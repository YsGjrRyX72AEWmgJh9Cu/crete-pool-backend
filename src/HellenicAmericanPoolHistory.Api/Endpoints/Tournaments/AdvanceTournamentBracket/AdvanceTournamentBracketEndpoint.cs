using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class AdvanceTournamentBracketEndpoint
{
    public static IEndpointRouteBuilder MapAdvanceTournamentBracketEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments/{id:guid}/bracket/advance",
            async Task<NoContent>(
                Guid id,
                AdvanceTournamentBracketHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("AdvanceTournamentBracket")
            .WithSummary("Advances a tournament bracket.")
            .WithDescription(
                "Advances a single-elimination tournament to the next round.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
