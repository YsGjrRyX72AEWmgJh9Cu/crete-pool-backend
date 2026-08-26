using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;
using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.TournamentSeries.GetTournamentSeries;

public sealed class GetTournamentSeriesEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetTournamentSeriesEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTournamentSeries_Should_Return_Ok_And_Tournament_Series()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organization = Organization.Create(
            $"API Get Tournament Series Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        var suffix = Guid.NewGuid().ToString("N");

        var seriesB = TournamentSeriesEntity.Create(
            organization.Id,
            $"B Series {suffix}");

        var seriesA = TournamentSeriesEntity.Create(
            organization.Id,
            $"A Series {suffix}");

        dbContext.TournamentSeries.AddRange(
            seriesB,
            seriesA);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/tournament-series");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<
                    IReadOnlyList<GetTournamentSeriesResponse>>();

        Assert.NotNull(responseBody);

        var testSeries = responseBody
            .Where(series =>
                series.OrganizationId == organization.Id.Value)
            .ToList();

        Assert.Equal(
            2,
            testSeries.Count);

        Assert.Equal(
            $"A Series {suffix}",
            testSeries[0].Name);

        Assert.Equal(
            $"B Series {suffix}",
            testSeries[1].Name);

        Assert.Equal(
            seriesA.Id.Value,
            testSeries[0].Id);

        Assert.Equal(
            seriesB.Id.Value,
            testSeries[1].Id);
    }
}
