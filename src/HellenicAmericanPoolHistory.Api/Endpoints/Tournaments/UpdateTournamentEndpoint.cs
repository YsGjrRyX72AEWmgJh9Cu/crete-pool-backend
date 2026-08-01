using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class UpdateTournamentEndpoint
{
    public static IEndpointRouteBuilder MapUpdateTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "/tournaments/{id:guid}",
            async Task<NoContent>(
                Guid id,
                UpdateTournamentCommand command,
                UpdateTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateTournamentCommand>>()
            .WithName("UpdateTournament")
            .WithSummary("Updates an existing tournament.")
            .WithDescription("Updates an existing tournament.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}