using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence;

/// <summary>
/// Creates <see cref="ApplicationDbContext"/> instances for EF Core design-time tools.
/// </summary>
public sealed class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory;Username=postgres;Password=postgres")
            .Options;

        return new ApplicationDbContext(options);
    }
}