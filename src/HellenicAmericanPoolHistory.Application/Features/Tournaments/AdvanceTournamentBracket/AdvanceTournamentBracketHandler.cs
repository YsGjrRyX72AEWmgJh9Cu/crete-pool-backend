using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;

/// <summary>
/// Handles tournament bracket advancement.
/// </summary>
public sealed class AdvanceTournamentBracketHandler
{
    private readonly IAdvanceTournamentBracketPort _port;

    public AdvanceTournamentBracketHandler(
        IAdvanceTournamentBracketPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken = default)
    {
        await _port.AdvanceAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}
