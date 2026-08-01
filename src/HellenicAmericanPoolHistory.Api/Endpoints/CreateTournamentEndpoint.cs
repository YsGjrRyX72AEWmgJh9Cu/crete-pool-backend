using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

public static class CreateTournamentEndpoint
{
    public static IEndpointRouteBuilder MapCreateTournamentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournaments",
            async Task<Created<CreateTournamentResponse>> (
                CreateTournamentCommand command,
                CreateTournamentHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(command, cancellationToken);

                return TypedResults.Created(
                    $"/tournaments/{response.TournamentId}",
                    response);
            })
            .AddEndpointFilter<ValidationFilter<CreateTournamentCommand>>()
            .WithName("CreateTournament")
            .WithSummary("Creates a new tournament.")
            .WithDescription("Creates a new tournament and returns its identifier.")
            .Produces<CreateTournamentResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}