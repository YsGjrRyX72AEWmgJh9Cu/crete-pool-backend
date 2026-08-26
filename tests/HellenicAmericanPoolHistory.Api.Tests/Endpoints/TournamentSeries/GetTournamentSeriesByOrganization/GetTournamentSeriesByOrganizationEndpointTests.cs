using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.TournamentSeries.GetTournamentSeriesByOrganization;

public sealed class GetTournamentSeriesByOrganizationEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetTournamentSeriesByOrganizationEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTournamentSeriesByOrganization_Should_Return_Ok_And_Tournament_Series()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organization = OrganizationEntity.Create(
            $"API Test Organization {Guid.NewGuid():N}");

        var otherOrganization = OrganizationEntity.Create(
            $"API Test Other Organization {Guid.NewGuid():N}");

        var firstSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"AAA API Test Series {Guid.NewGuid():N}");

        var secondSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"ZZZ API Test Series {Guid.NewGuid():N}");

        var otherSeries = TournamentSeriesEntity.Create(
            otherOrganization.Id,
            $"API Test Other Series {Guid.NewGuid():N}");

        dbContext.Organizations.AddRange(
            organization,
            otherOrganization);

        dbContext.TournamentSeries.AddRange(
            secondSeries,
            otherSeries,
            firstSeries);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/organizations/{organization.Id.Value}/tournament-series");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content.ReadFromJsonAsync<
                IReadOnlyList<GetTournamentSeriesByOrganizationResponse>>();

        Assert.NotNull(responseBody);

        Assert.Equal(
            2,
            responseBody.Count);

        Assert.Equal(
            firstSeries.Id.Value,
            responseBody[0].Id);

        Assert.Equal(
            firstSeries.Name,
            responseBody[0].Name);

        Assert.Equal(
            organization.Id.Value,
            responseBody[0].OrganizationId);

        Assert.Equal(
            secondSeries.Id.Value,
            responseBody[1].Id);

        Assert.Equal(
            secondSeries.Name,
            responseBody[1].Name);

        Assert.Equal(
            organization.Id.Value,
            responseBody[1].OrganizationId);

        Assert.DoesNotContain(
            responseBody,
            item => item.Id == otherSeries.Id.Value);
    }

    [Fact]
    public async Task GetTournamentSeriesByOrganization_Should_Return_Empty_List_When_No_Series_Exist()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organization = OrganizationEntity.Create(
            $"API Test Empty Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            $"/organizations/{organization.Id.Value}/tournament-series");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content.ReadFromJsonAsync<
                IReadOnlyList<GetTournamentSeriesByOrganizationResponse>>();

        Assert.NotNull(responseBody);
        Assert.Empty(responseBody);
    }
}
