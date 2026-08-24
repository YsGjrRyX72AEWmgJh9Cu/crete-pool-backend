using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;

/// <summary>
/// Advances tournament brackets in persistence.
/// </summary>
public interface IAdvanceTournamentBracketPort
{
    Task AdvanceAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}
