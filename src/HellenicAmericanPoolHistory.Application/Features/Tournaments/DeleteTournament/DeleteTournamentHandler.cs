using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;

/// <summary>
/// Handles tournament deletion.
/// </summary>
public sealed class DeleteTournamentHandler
{
    private readonly IDeleteTournamentPort _port;

    public DeleteTournamentHandler(
        IDeleteTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken)
    {
        await _port.DeleteAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}