using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;

/// <summary>
/// Starts tournaments in persistence.
/// </summary>
public interface IStartTournamentPort
{
    Task StartAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}