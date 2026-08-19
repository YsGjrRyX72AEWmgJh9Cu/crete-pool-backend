using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

public static class UpdateParticipationEndpoint
{
    public static IEndpointRouteBuilder MapUpdateParticipationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "/participations/{id:guid}",
            async Task<NoContent>(
                Guid id,
                UpdateParticipationRequest request,
                UpdateParticipationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateParticipationCommand(
                    new ParticipationId(id),
                    request.Seed,
                    request.Status);

                await handler.HandleAsync(
                    command,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateParticipationRequest>>()
            .WithName("UpdateParticipation")
            .WithSummary("Updates an existing participation.")
            .WithDescription("Updates an existing participation.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
