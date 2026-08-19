using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;

/// <summary>
/// Handles tournament completion requests.
/// </summary>
public sealed class CompleteTournamentHandler
{
    private readonly ICompleteTournamentPort _port;

    public CompleteTournamentHandler(
        ICompleteTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken)
    {
        await _port.CompleteAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}
