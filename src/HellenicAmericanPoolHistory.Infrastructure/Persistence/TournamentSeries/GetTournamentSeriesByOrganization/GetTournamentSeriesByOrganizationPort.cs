using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.TournamentSeries.GetTournamentSeriesByOrganization;

/// <summary>
/// Retrieves tournament series belonging to an organization.
/// </summary>
public sealed class GetTournamentSeriesByOrganizationPort(
    ApplicationDbContext dbContext)
    : IGetTournamentSeriesByOrganizationPort
{
    public async Task<IReadOnlyList<GetTournamentSeriesByOrganizationResponse>> GetAllAsync(
        OrganizationId organizationId,
        CancellationToken cancellationToken)
    {
        return await dbContext.TournamentSeries
            .AsNoTracking()
            .Where(series => series.OrganizationId == organizationId)
            .OrderBy(series => series.Name)
            .Select(
                series => new GetTournamentSeriesByOrganizationResponse(
                    series.Id.Value,
                    series.Name,
                    series.OrganizationId.Value))
            .ToListAsync(cancellationToken);
    }
}
