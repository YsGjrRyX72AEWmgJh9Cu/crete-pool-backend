namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;

/// <summary>
/// Handles requests to retrieve all tournaments.
/// </summary>
public sealed class GetTournamentsHandler(IGetTournamentsPort port)
{
    public Task<IReadOnlyList<GetTournamentsResponse>> HandleAsync(
        CancellationToken cancellationToken)
    {
        return port.GetAllAsync(cancellationToken);
    }
}