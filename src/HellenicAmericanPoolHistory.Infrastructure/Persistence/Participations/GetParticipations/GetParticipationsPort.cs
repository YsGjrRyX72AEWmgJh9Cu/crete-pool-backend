using System.Linq;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.GetParticipations;

/// <summary>
/// Retrieves participations from the database.
/// </summary>
public sealed class GetParticipationsPort : IGetParticipationsPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetParticipationsPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<GetParticipationsResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Participations
            .AsNoTracking()
            .OrderBy(participation => participation.Tournament.Name)
            .ThenBy(participation => participation.Player.LastName)
            .ThenBy(participation => participation.Player.FirstName)
            .Select(participation => new GetParticipationsResponse(
                participation.Id.Value,
                participation.Player.FullName,
                participation.Tournament.Name,
                participation.RegistrationDate,
                participation.Seed,
                participation.Status.ToString()))
            .ToListAsync(cancellationToken);
    }
}