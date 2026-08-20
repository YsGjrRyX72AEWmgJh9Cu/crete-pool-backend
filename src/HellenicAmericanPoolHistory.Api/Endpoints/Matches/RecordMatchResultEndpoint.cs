using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Matches;

public static class RecordMatchResultEndpoint
{
    public static IEndpointRouteBuilder MapRecordMatchResultEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/matches/{id:guid}/result",
            async Task<NoContent>(
                Guid id,
                RecordMatchResultRequest request,
                [FromServices] RecordMatchResultHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new RecordMatchResultCommand(
                    id,
                    request.WinnerParticipationId,
                    request.Participant1Score,
                    request.Participant2Score);

                await handler.HandleAsync(
                    command,
                    cancellationToken);

                return TypedResults.NoContent();
            })
            .AddEndpointFilter<
                ValidationFilter<RecordMatchResultRequest>>()
            .WithName("RecordMatchResult")
            .WithSummary("Records the result of a match.")
            .WithDescription(
                "Records the winner and scores for an existing match.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
