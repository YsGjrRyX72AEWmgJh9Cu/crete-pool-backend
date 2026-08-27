using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Organizations.GetOrganization;
using Microsoft.EntityFrameworkCore;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Organizations.GetOrganization;

public sealed class GetOrganizationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GetByIdAsync_Should_Return_Organization()
    {
        await using var dbContext = CreateDbContext();

        var organization = OrganizationEntity.Create(
            $"Get Organization Test {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var port = new GetOrganizationPort(dbContext);

        var result = await port.GetByIdAsync(
            organization.Id.Value,
            CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(
            organization.Id.Value,
            result.Id);

        Assert.Equal(
            organization.Name,
            result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_When_Organization_Does_Not_Exist_Should_Return_Null()
    {
        await using var dbContext = CreateDbContext();

        var port = new GetOrganizationPort(dbContext);

        var result = await port.GetByIdAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.Null(result);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
