using System.Net;
using System.Net.Http.Json;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Api.Tests.Endpoints.TournamentSeries.CreateTournamentSeries;

public sealed class CreateTournamentSeriesEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateTournamentSeriesEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateTournamentSeries_Should_Return_Created_And_Persist_Tournament_Series()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var organization = OrganizationEntity.Create(
            $"API Test Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var command = new CreateTournamentSeriesCommand(
            $"API Test Tournament Series {Guid.NewGuid():N}",
            organization.Id.Value);

        var response = await client.PostAsJsonAsync(
            "/tournament-series",
            command);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var responseBody =
            await response.Content
                .ReadFromJsonAsync<CreateTournamentSeriesResponse>();

        Assert.NotNull(responseBody);

        Assert.NotEqual(
            Guid.Empty,
            responseBody.TournamentSeriesId);

        Assert.Equal(
            $"/tournament-series/{responseBody.TournamentSeriesId}",
            response.Headers.Location?.ToString());

        var persistedTournamentSeries =
            await dbContext.TournamentSeries
                .AsNoTracking()
                .SingleAsync(
                    item => item.Id ==
                        new HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeriesId(
                            responseBody.TournamentSeriesId));

        Assert.Equal(
            command.Name,
            persistedTournamentSeries.Name);

        Assert.Equal(
            command.OrganizationId,
            persistedTournamentSeries.OrganizationId.Value);
    }
}
