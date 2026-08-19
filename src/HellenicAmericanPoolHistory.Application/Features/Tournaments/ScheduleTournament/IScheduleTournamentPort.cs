using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;

/// <summary>
/// Schedules tournaments.
/// </summary>
public interface IScheduleTournamentPort
{
    Task ScheduleAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}
