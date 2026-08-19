namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;

/// <summary>
/// Handles requests to retrieve a participation.
/// </summary>
public sealed class GetParticipationHandler(
    IGetParticipationPort port)
{
    /// <summary>
    /// Handles the request.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The participation if found; otherwise <see langword="null" />.
    /// </returns>
    public Task<GetParticipationResponse?> HandleAsync(
        GetParticipationQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetByIdAsync(
            query.ParticipationId,
            cancellationToken);
    }
}