using HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class ScheduleTournamentEndpoint
{
    public static IEndpointRouteBuilder MapScheduleTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments/{id:guid}/schedule",
            async Task<NoContent>(
                Guid id,
                ScheduleTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("ScheduleTournament")
            .WithSummary("Schedules a tournament.")
            .WithDescription("Moves a tournament from Draft to Scheduled.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
