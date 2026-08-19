using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

public static class CreateParticipationEndpoint
{
    public static IEndpointRouteBuilder MapCreateParticipationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/participations",
            async Task<Created<CreateParticipationResponse>>(
                CreateParticipationCommand command,
                CreateParticipationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(command, cancellationToken);

                return TypedResults.Created(
                    $"/participations/{response.Id}",
                    response);
            })
            .AddEndpointFilter<ValidationFilter<CreateParticipationCommand>>()
            .WithName("CreateParticipation")
            .WithSummary("Creates a new participation.")
            .WithDescription(
                "Creates a new participation and returns its identifier.")
            .Produces<CreateParticipationResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}