using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;

/// <summary>
/// Handles tournament scheduling.
/// </summary>
public sealed class ScheduleTournamentHandler
{
    private readonly IScheduleTournamentPort _port;

    public ScheduleTournamentHandler(
        IScheduleTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken)
    {
        await _port.ScheduleAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}
