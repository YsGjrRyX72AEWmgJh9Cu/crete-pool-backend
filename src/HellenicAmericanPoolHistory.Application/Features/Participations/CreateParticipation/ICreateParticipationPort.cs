using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;

/// <summary>
/// Defines the persistence operations required to create a participation.
/// </summary>
public interface ICreateParticipationPort
{
    Task<ParticipationId> CreateAsync(
        Participation participation,
        CancellationToken cancellationToken = default);
}