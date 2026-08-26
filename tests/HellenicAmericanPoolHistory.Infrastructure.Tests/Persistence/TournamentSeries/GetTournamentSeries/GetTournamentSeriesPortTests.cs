using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.TournamentSeries.GetTournamentSeries;
using Microsoft.EntityFrameworkCore;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.TournamentSeries.GetTournamentSeries;

public sealed class GetTournamentSeriesPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_Tournament_Series_Ordered_By_Name()
    {
        await using var dbContext = CreateDbContext();

        var organization = Organization.Create(
            $"Infrastructure Test Organization {Guid.NewGuid():N}");

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

        var port = new GetTournamentSeriesPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        var testSeries = result
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

        Assert.Equal(
            organization.Id.Value,
            testSeries[0].OrganizationId);

        Assert.Equal(
            organization.Id.Value,
            testSeries[1].OrganizationId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List_When_No_Tournament_Series_Exist()
    {
        await using var dbContext = CreateDbContext();

        var organization = Organization.Create(
            $"Infrastructure Test Empty Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var port = new GetTournamentSeriesPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        var testSeries = result
            .Where(series =>
                series.OrganizationId == organization.Id.Value)
            .ToList();

        Assert.Empty(testSeries);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
