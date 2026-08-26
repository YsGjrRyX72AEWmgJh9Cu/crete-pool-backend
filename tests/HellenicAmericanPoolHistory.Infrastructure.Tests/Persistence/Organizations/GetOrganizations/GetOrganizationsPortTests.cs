using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Organizations.GetOrganizations;
using Microsoft.EntityFrameworkCore;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Organizations.GetOrganizations;

public sealed class GetOrganizationsPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetAllAsync_Should_Return_Organizations_Ordered_By_Name()
    {
        await using var dbContext = CreateDbContext();

        var uniquePrefix = $"Get Organizations Test {Guid.NewGuid():N}";

        var organizationB = OrganizationEntity.Create(
            $"{uniquePrefix} B");

        var organizationA = OrganizationEntity.Create(
            $"{uniquePrefix} A");

        dbContext.Organizations.Add(organizationB);
        dbContext.Organizations.Add(organizationA);

        await dbContext.SaveChangesAsync();

        var port = new GetOrganizationsPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        var testOrganizations = result
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

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Organizations()
    {
        await using var dbContext = CreateDbContext();

        var organizationA = OrganizationEntity.Create(
            $"Get Organizations Test A {Guid.NewGuid():N}");

        var organizationB = OrganizationEntity.Create(
            $"Get Organizations Test B {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organizationA);
        dbContext.Organizations.Add(organizationB);

        await dbContext.SaveChangesAsync();

        var port = new GetOrganizationsPort(dbContext);

        var result = await port.GetAllAsync(
            CancellationToken.None);

        Assert.Contains(
            result,
            organization => organization.Id == organizationA.Id.Value);

        Assert.Contains(
            result,
            organization => organization.Id == organizationB.Id.Value);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
