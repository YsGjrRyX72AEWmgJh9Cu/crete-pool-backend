namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;

/// <summary>
/// Defines the contract for retrieving participations.
/// </summary>
public interface IGetParticipationsPort
{
    /// <summary>
    /// Retrieves all participations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of participations.</returns>
    Task<IReadOnlyCollection<GetParticipationsResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}