using HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class StartTournamentEndpoint
{
    public static IEndpointRouteBuilder MapStartTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments/{id:guid}/start",
            async Task<NoContent>(
                Guid id,
                StartTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("StartTournament")
            .WithSummary("Starts a tournament.")
            .WithDescription(
                "Moves a tournament from Scheduled to InProgress.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}

