using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class GetTournamentBracketEndpoint
{
    public static IEndpointRouteBuilder MapGetTournamentBracketEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/tournaments/{id:guid}/bracket",
            async Task<Results<
                Ok<GetTournamentBracketResponse>,
                NotFound>>(
                Guid id,
                [FromServices] GetTournamentBracketHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(
                    new GetTournamentBracketQuery(id),
                    cancellationToken);

                return response is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(response);
            })
            .WithName("GetTournamentBracket")
            .WithSummary("Gets a tournament bracket.")
            .WithDescription("Returns the matches grouped by tournament round.")
            .Produces<GetTournamentBracketResponse>(
                StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
