using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;

/// <summary>
/// Generates tournament brackets in persistence.
/// </summary>
public interface IGenerateTournamentBracketPort
{
    Task GenerateAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}
