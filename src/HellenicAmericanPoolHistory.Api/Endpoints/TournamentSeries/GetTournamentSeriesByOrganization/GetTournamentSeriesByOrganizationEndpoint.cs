using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Domain.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.TournamentSeries.GetTournamentSeriesByOrganization;

public static class GetTournamentSeriesByOrganizationEndpoint
{
    public static IEndpointRouteBuilder MapGetTournamentSeriesByOrganizationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/organizations/{organizationId:guid}/tournament-series",
            async Task<Ok<IReadOnlyList<GetTournamentSeriesByOrganizationResponse>>> (
                Guid organizationId,
                GetTournamentSeriesByOrganizationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(
                        new OrganizationId(organizationId),
                        cancellationToken);

                return TypedResults.Ok(response);
            })
            .WithName("GetTournamentSeriesByOrganization")
            .WithSummary("Gets tournament series for an organization.")
            .WithDescription(
                "Returns all tournament series belonging to the specified organization.")
            .Produces<IReadOnlyList<GetTournamentSeriesByOrganizationResponse>>(
                StatusCodes.Status200OK);

        return endpoints;
    }
}
