using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Organizations.GetOrganizations;

/// <summary>
/// Retrieves organizations from persistence.
/// </summary>
public sealed class GetOrganizationsPort : IGetOrganizationsPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetOrganizationsPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GetOrganizationsResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Organizations
            .AsNoTracking()
            .OrderBy(organization => organization.Name)
            .Select(
                organization => new GetOrganizationsResponse(
                    organization.Id.Value,
                    organization.Name))
            .ToListAsync(cancellationToken);
    }
}
