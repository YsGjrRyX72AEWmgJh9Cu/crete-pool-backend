using HellenicAmericanPoolHistory.Domain.Organization;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.TournamentSeries.CreateTournamentSeries;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.TournamentSeries.CreateTournamentSeries;

public sealed class CreateTournamentSeriesPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task SaveAsync_With_Valid_TournamentSeries_Should_Persist_TournamentSeries()
    {
        await using var dbContext = CreateDbContext();

        var organization = Organization.Create(
            $"Infrastructure Test Organization {Guid.NewGuid():N}");

        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync();

        var tournamentSeries = TournamentSeriesEntity.Create(
            organization.Id,
            $"Infrastructure Test Tournament Series {Guid.NewGuid():N}");

        var port = new CreateTournamentSeriesPort(dbContext);

        await port.SaveAsync(
            tournamentSeries,
            CancellationToken.None);

        var persistedTournamentSeries =
            await dbContext.TournamentSeries
                .SingleAsync(
                    series => series.Id == tournamentSeries.Id);

        Assert.Equal(
            tournamentSeries.Id,
            persistedTournamentSeries.Id);

        Assert.Equal(
            organization.Id,
            persistedTournamentSeries.OrganizationId);

        Assert.Equal(
            tournamentSeries.Name,
            persistedTournamentSeries.Name);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
