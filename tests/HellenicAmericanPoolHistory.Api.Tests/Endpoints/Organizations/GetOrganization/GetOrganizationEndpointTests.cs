using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;
using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Organizations.GetOrganization;

public sealed class GetOrganizationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetOrganizationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrganization_Should_Return_Ok_And_Organization()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organization = Organization.Create(
            $"API Get Organization Test {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/organizations/{organization.Id.Value}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<GetOrganizationResponse>();

        Assert.NotNull(responseBody);

        Assert.Equal(
            organization.Id.Value,
            responseBody.Id);

        Assert.Equal(
            organization.Name,
            responseBody.Name);
    }

    [Fact]
    public async Task GetOrganization_When_Organization_Does_Not_Exist_Should_Return_NotFound()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organizationId = Guid.NewGuid();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/organizations/{organizationId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
