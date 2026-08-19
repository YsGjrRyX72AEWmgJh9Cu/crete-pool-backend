using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

public static class CreateMatchEndpoint
{
    public static IEndpointRouteBuilder MapCreateMatchEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/matches",
            async Task<Created<CreateMatchResponse>>(
                CreateMatchCommand command,
                CreateMatchHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(
                        command,
                        cancellationToken);

                return TypedResults.Created(
                    $"/matches/{response.Id}",
                    response);
            })
            .AddEndpointFilter<ValidationFilter<CreateMatchCommand>>()
            .WithName("CreateMatch")
            .WithSummary("Creates a new match.")
            .WithDescription(
                "Creates a new match and returns its identifier.")
            .Produces<CreateMatchResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
