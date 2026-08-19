using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;

/// <summary>
/// Completes tournaments in persistence.
/// </summary>
public interface ICompleteTournamentPort
{
    Task CompleteAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}
