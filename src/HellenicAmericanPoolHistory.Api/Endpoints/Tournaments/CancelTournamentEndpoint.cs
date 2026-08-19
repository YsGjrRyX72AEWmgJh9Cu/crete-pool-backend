using HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class CancelTournamentEndpoint
{
    public static IEndpointRouteBuilder MapCancelTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments/{id:guid}/cancel",
            async Task<NoContent>(
                Guid id,
                CancelTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("CancelTournament")
            .WithSummary("Cancels a tournament.")
            .WithDescription(
                "Moves a tournament from Draft or Scheduled to Cancelled.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
