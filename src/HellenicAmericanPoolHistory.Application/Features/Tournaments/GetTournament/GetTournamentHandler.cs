namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;

/// <summary>
/// Handles requests to retrieve a tournament.
/// </summary>
public sealed class GetTournamentHandler(IGetTournamentPort port)
{
    public Task<GetTournamentResponse?> HandleAsync(
        GetTournamentQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return port.GetByIdAsync(
            query.TournamentId,
            cancellationToken);
    }
}