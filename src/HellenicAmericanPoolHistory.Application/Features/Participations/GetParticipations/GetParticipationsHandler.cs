namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;

/// <summary>
/// Handles requests to retrieve participations.
/// </summary>
public sealed class GetParticipationsHandler(
    IGetParticipationsPort port)
{
    /// <summary>
    /// Handles the request.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The collection of participations.
    /// </returns>
    public Task<IReadOnlyCollection<GetParticipationsResponse>> HandleAsync(
        GetParticipationsQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetAllAsync(cancellationToken);
    }
}