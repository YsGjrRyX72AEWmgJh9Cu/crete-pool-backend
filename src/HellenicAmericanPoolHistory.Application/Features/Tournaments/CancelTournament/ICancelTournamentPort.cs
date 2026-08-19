using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;

/// <summary>
/// Cancels tournaments in persistence.
/// </summary>
public interface ICancelTournamentPort
{
    Task CancelAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}
