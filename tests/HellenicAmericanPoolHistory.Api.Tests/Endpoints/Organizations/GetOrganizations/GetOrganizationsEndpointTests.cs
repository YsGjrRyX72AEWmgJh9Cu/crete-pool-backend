using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;
using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Organizations.GetOrganizations;

public sealed class GetOrganizationsEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetOrganizationsEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrganizations_Should_Return_Ok_And_Organizations()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var uniquePrefix =
            $"API Get Organizations Test {Guid.NewGuid():N}";

        var organizationB = Organization.Create(
            $"{uniquePrefix} B");

        var organizationA = Organization.Create(
            $"{uniquePrefix} A");

        dbContext.Organizations.Add(organizationB);
        dbContext.Organizations.Add(organizationA);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/organizations");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyList<GetOrganizationsResponse>>();

        Assert.NotNull(responseBody);

        var testOrganizations = responseBody
            .Where(organization =>
                organization.Name.StartsWith(uniquePrefix))
            .ToList();

        Assert.Equal(
            2,
            testOrganizations.Count);

        Assert.Equal(
            organizationA.Id.Value,
            testOrganizations[0].Id);

        Assert.Equal(
            organizationA.Name,
            testOrganizations[0].Name);

        Assert.Equal(
            organizationB.Id.Value,
            testOrganizations[1].Id);

        Assert.Equal(
            organizationB.Name,
            testOrganizations[1].Name);
    }
}
