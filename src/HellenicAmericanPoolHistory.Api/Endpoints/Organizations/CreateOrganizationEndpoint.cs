using HellenicAmericanPoolHistory.Api.Filters;
using HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Organizations;

public static class CreateOrganizationEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrganizationEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/organizations",
            async Task<Created<CreateOrganizationResponse>> (
                CreateOrganizationCommand command,
                CreateOrganizationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.HandleAsync(
                        command,
                        cancellationToken);

                return TypedResults.Created(
                    $"/organizations/{response.OrganizationId}",
                    response);
            })
            .AddEndpointFilter<
                ValidationFilter<CreateOrganizationCommand>>()
            .WithName("CreateOrganization")
            .WithSummary("Creates a new organization.")
            .WithDescription(
                "Creates a new organization and returns its identifier.")
            .Produces<CreateOrganizationResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}
