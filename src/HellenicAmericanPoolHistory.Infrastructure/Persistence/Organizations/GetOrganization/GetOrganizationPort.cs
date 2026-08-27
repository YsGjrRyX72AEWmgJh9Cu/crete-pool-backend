using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Organizations.GetOrganization;

/// <summary>
/// Retrieves organizations from the database.
/// </summary>
public sealed class GetOrganizationPort : IGetOrganizationPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetOrganizationPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<GetOrganizationResponse?> GetByIdAsync(
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Organizations
            .AsNoTracking()
            .Where(o => o.Id == new Domain.Organization.OrganizationId(organizationId))
            .Select(o => new GetOrganizationResponse(
                o.Id.Value,
                o.Name))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
