namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;

/// <summary>
/// Retrieves all tournaments.
/// </summary>
public interface IGetTournamentsPort
{
    Task<IReadOnlyList<GetTournamentsResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}