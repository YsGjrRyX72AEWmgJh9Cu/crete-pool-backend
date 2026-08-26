using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.TournamentSeries.GetTournamentSeriesByOrganization;
using Microsoft.EntityFrameworkCore;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.TournamentSeries.GetTournamentSeriesByOrganization;

public sealed class GetTournamentSeriesByOrganizationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_Only_Tournament_Series_Belonging_To_Organization()
    {
        await using var dbContext = CreateDbContext();

        var organization = OrganizationEntity.Create(
            $"Infrastructure Test Organization {Guid.NewGuid():N}");

        var otherOrganization = OrganizationEntity.Create(
            $"Infrastructure Test Other Organization {Guid.NewGuid():N}");

        dbContext.Organizations.AddRange(
            organization,
            otherOrganization);

        var matchingSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"Infrastructure Test Tournament Series {Guid.NewGuid():N}");

        var otherSeries = TournamentSeriesEntity.Create(
            otherOrganization.Id,
            $"Infrastructure Test Tournament Series {Guid.NewGuid():N}");

        dbContext.TournamentSeries.AddRange(
            matchingSeries,
            otherSeries);

        await dbContext.SaveChangesAsync();

        var port =
            new GetTournamentSeriesByOrganizationPort(dbContext);

        var result = await port.GetAllAsync(
            organization.Id,
            CancellationToken.None);

        Assert.Single(result);

        var response = result[0];

        Assert.Equal(
            matchingSeries.Id.Value,
            response.Id);

        Assert.Equal(
            matchingSeries.Name,
            response.Name);

        Assert.Equal(
            organization.Id.Value,
            response.OrganizationId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Tournament_Series_Ordered_By_Name()
    {
        await using var dbContext = CreateDbContext();

        var organization = OrganizationEntity.Create(
            $"Infrastructure Test Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        var firstSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"AAA Infrastructure Test Series {Guid.NewGuid():N}");

        var secondSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"ZZZ Infrastructure Test Series {Guid.NewGuid():N}");

        dbContext.TournamentSeries.AddRange(
            secondSeries,
            firstSeries);

        await dbContext.SaveChangesAsync();

        var port =
            new GetTournamentSeriesByOrganizationPort(dbContext);

        var result = await port.GetAllAsync(
            organization.Id,
            CancellationToken.None);

        Assert.Equal(
            2,
            result.Count);

        Assert.Equal(
            firstSeries.Name,
            result[0].Name);

        Assert.Equal(
            secondSeries.Name,
            result[1].Name);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List_When_Organization_Has_No_Tournament_Series()
    {
        await using var dbContext = CreateDbContext();

        var organization = OrganizationEntity.Create(
            $"Infrastructure Test Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var port =
            new GetTournamentSeriesByOrganizationPort(dbContext);

        var result = await port.GetAllAsync(
            organization.Id,
            CancellationToken.None);

        Assert.Empty(result);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        return new ApplicationDbContext(options);
    }
}
