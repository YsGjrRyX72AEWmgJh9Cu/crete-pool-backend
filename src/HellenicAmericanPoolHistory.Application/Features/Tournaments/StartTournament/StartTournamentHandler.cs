using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;

/// <summary>
/// Handles tournament start requests.
/// </summary>
public sealed class StartTournamentHandler
{
    private readonly IStartTournamentPort _port;

    public StartTournamentHandler(
        IStartTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken)
    {
        await _port.StartAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}
