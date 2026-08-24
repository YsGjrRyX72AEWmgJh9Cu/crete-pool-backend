using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;

/// <summary>
/// Handles tournament bracket generation.
/// </summary>
public sealed class GenerateTournamentBracketHandler
{
    private readonly IGenerateTournamentBracketPort _port;

    public GenerateTournamentBracketHandler(
        IGenerateTournamentBracketPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        CancellationToken cancellationToken = default)
    {
        await _port.GenerateAsync(
            new TournamentId(tournamentId),
            cancellationToken);
    }
}
