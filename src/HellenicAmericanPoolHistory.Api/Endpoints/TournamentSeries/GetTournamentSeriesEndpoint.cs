using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.TournamentSeries;

public static class GetTournamentSeriesEndpoint
{
    public static IEndpointRouteBuilder MapGetTournamentSeriesEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/tournament-series",
            async Task<Ok<IReadOnlyList<GetTournamentSeriesResponse>>>(
                [FromServices] GetTournamentSeriesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(cancellationToken);

                return TypedResults.Ok(response);
            })
            .WithName("GetTournamentSeries")
            .WithSummary("Gets all tournament series.")
            .WithDescription("Returns all tournament series.")
            .Produces<IReadOnlyList<GetTournamentSeriesResponse>>(
                StatusCodes.Status200OK);

        return endpoints;
    }
}
