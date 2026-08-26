using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.TournamentSeries;

public static class CreateTournamentSeriesEndpoint
{
    public static IEndpointRouteBuilder MapCreateTournamentSeriesEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/tournament-series",
            async Task<Created<CreateTournamentSeriesResponse>> (
                CreateTournamentSeriesCommand command,
                CreateTournamentSeriesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(
                        command,
                        cancellationToken);

                return TypedResults.Created(
                    $"/tournament-series/{response.TournamentSeriesId}",
                    response);
            })
            .AddEndpointFilter<
                ValidationFilter<CreateTournamentSeriesCommand>>()
            .WithName("CreateTournamentSeries")
            .WithSummary("Creates a new tournament series.")
            .WithDescription(
                "Creates a new tournament series and returns its identifier.")
            .Produces<CreateTournamentSeriesResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}
