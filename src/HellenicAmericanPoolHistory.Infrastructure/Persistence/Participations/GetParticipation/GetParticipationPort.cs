using System.Linq;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.GetParticipation;

/// <summary>
/// Retrieves participations from the database.
/// </summary>
public sealed class GetParticipationPort : IGetParticipationPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetParticipationPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<GetParticipationResponse?> GetByIdAsync(
        ParticipationId participationId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Participations
            .AsNoTracking()
            .Where(participation => participation.Id == participationId)
            .Select(participation => new GetParticipationResponse(
                participation.Id.Value,
                participation.PlayerId.Value,
                participation.Player.FullName,
                participation.TournamentId.Value,
                participation.Tournament.Name,
                participation.RegistrationDate,
                participation.Seed,
                participation.Status.ToString()))
            .SingleOrDefaultAsync(cancellationToken);
    }
}