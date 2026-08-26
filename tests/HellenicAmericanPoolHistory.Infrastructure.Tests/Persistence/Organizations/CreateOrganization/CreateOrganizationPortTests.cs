using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Organizations.CreateOrganization;
using Microsoft.EntityFrameworkCore;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Organizations.CreateOrganization;

public sealed class CreateOrganizationPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task SaveAsync_With_Valid_Organization_Should_Persist_Organization()
    {
        await using var dbContext = CreateDbContext();

        var organization = OrganizationEntity.Create(
            $"Infrastructure Test Organization {Guid.NewGuid():N}");

        var port = new CreateOrganizationPort(dbContext);

        await port.SaveAsync(
            organization,
            CancellationToken.None);

        var persistedOrganization =
            await dbContext.Organizations
                .SingleAsync(
                    item => item.Id == organization.Id);

        Assert.Equal(
            organization.Id,
            persistedOrganization.Id);

        Assert.Equal(
            organization.Name,
            persistedOrganization.Name);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
