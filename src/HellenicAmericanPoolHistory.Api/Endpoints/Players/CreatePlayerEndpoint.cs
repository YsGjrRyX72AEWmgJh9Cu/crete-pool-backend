using HellenicAmericanPoolHistory.Api.Filters;
using Microsoft.AspNetCore.Http;
using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Players;

public static class CreatePlayerEndpoint
{
    public static IEndpointRouteBuilder MapCreatePlayerEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/players",
            async Task<Created<CreatePlayerResponse>> (
                CreatePlayerCommand command,
                CreatePlayerHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(command, cancellationToken);

                return TypedResults.Created(
                    $"/players/{response.Id}",
                    response);
            })
            .AddEndpointFilter<ValidationFilter<CreatePlayerCommand>>()
            .WithName("CreatePlayer")
            .WithSummary("Creates a new player.")
            .WithDescription("Creates a new player and returns its identifier.")
            .Produces<CreatePlayerResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}