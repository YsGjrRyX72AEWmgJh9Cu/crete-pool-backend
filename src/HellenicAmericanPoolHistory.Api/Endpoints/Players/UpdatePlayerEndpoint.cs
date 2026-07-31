using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Players;

public static class UpdatePlayerEndpoint
{
    public static IEndpointRouteBuilder MapUpdatePlayerEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "/players/{id:guid}",
            async Task<NoContent> (
                Guid id,
                UpdatePlayerCommand request,
                UpdatePlayerHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = request with
                {
                    Id = new PlayerId(id)
                };

                await handler.Handle(command, cancellationToken);

                return TypedResults.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdatePlayerCommand>>()
            .WithName("UpdatePlayer")
            .WithSummary("Updates an existing player.")
            .WithDescription("Updates an existing player.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}