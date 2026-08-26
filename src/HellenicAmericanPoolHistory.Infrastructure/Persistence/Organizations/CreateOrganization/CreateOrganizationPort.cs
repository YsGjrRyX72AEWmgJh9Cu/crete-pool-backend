using HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Organizations.CreateOrganization;

/// <summary>
/// Persists newly created organizations.
/// </summary>
public sealed class CreateOrganizationPort
    : ICreateOrganizationPort
{
    private readonly ApplicationDbContext _dbContext;

    public CreateOrganizationPort(
        ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task SaveAsync(
        OrganizationEntity organization,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(organization);

        await _dbContext.Organizations.AddAsync(
            organization,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}
