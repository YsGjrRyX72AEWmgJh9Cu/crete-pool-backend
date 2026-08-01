using HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class DeleteTournamentEndpoint
{
    public static IEndpointRouteBuilder MapDeleteTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/tournaments/{id:guid}",
            async Task<NoContent>(
                Guid id,
                DeleteTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("DeleteTournament")
            .WithSummary("Deletes an existing tournament.")
            .WithDescription("Deletes an existing tournament.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}