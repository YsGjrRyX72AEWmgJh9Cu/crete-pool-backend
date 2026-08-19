using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;

/// <summary>
/// Defines the contract for retrieving a participation.
/// </summary>
public interface IGetParticipationPort
{
    /// <summary>
    /// Retrieves a participation by identifier.
    /// </summary>
    /// <param name="participationId">The participation identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The participation if found; otherwise <see langword="null" />.
    /// </returns>
    Task<GetParticipationResponse?> GetByIdAsync(
        ParticipationId participationId,
        CancellationToken cancellationToken);
}