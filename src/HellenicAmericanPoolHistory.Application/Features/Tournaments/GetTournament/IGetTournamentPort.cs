namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;

/// <summary>
/// Retrieves a tournament.
/// </summary>
public interface IGetTournamentPort
{
    Task<GetTournamentResponse?> GetByIdAsync(
        Guid tournamentId,
        CancellationToken cancellationToken);
}