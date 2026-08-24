using HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class GenerateTournamentBracketEndpoint
{
    public static IEndpointRouteBuilder MapGenerateTournamentBracketEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments/{id:guid}/bracket",
            async Task<NoContent>(
                Guid id,
                GenerateTournamentBracketHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("GenerateTournamentBracket")
            .WithSummary("Generates a tournament bracket.")
            .WithDescription(
                "Generates the first round of a single-elimination tournament.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
