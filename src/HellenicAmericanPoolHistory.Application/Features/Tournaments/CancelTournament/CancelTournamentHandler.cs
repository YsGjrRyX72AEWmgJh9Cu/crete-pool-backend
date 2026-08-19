using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;

/// <summary>
/// Handles tournament cancellation requests.
/// </summary>
public sealed class CancelTournamentHandler
{
    private readonly ICancelTournamentPort _port;

    public CancelTournamentHandler(
        ICancelTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken)
    {
        await _port.CancelAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}
