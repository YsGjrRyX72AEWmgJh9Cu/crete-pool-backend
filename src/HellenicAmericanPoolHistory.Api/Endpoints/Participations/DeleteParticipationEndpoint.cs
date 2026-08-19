using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

public static class DeleteParticipationEndpoint
{
    public static IEndpointRouteBuilder MapDeleteParticipationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/participations/{id:guid}",
            async Task<NoContent>(
                Guid id,
                DeleteParticipationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteParticipationCommand(
                    new ParticipationId(id));

                await handler.Handle(command, cancellationToken);

                return TypedResults.NoContent();
            })
            .WithName("DeleteParticipation")
            .WithSummary("Deletes an existing participation.")
            .WithDescription("Deletes an existing participation.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
