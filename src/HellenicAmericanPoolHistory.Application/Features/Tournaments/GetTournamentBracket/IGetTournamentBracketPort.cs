using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;

/// <summary>
/// Retrieves tournament brackets from persistence.
/// </summary>
public interface IGetTournamentBracketPort
{
    Task<GetTournamentBracketResponse?> GetByTournamentIdAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}
