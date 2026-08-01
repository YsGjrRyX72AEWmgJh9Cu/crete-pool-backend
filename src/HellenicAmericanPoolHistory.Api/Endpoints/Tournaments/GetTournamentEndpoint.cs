using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class GetTournamentEndpoint
{
    public static IEndpointRouteBuilder MapGetTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/tournaments/{id:guid}",
            async Task<Results<Ok<GetTournamentResponse>, NotFound>>(
                Guid id,
                GetTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(
                    new GetTournamentQuery(id),
                    cancellationToken);

                return response is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(response);
            })
            .WithName("GetTournament")
            .WithSummary("Gets a tournament by identifier.")
            .WithDescription("Returns a tournament.")
            .Produces<GetTournamentResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}