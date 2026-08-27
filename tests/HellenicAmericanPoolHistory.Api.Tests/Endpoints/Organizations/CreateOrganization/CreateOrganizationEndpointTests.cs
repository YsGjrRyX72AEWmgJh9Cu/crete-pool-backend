using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.Organizations.CreateOrganization;

public sealed class CreateOrganizationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateOrganizationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateOrganization_Should_Return_Created_And_Persist_Organization()
    {
        var client = _factory.CreateClient();

        var command = new CreateOrganizationCommand(
            $"API Test Organization {Guid.NewGuid():N}");

        var response = await client.PostAsJsonAsync(
            "/organizations",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<CreateOrganizationResponse>();

        Assert.NotNull(responseBody);

        Assert.NotEqual(
            Guid.Empty,
            responseBody.OrganizationId);

        Assert.Equal(
            $"/organizations/{responseBody.OrganizationId}",
            response.Headers.Location?.ToString());

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organizations =
            await dbContext.Organizations
                .AsNoTracking()
                .ToListAsync();

        var organization =
            Assert.Single(
                organizations,
                item => item.Id.Value == responseBody.OrganizationId);

        Assert.Equal(
            command.Name,
            organization.Name);
    }
}
