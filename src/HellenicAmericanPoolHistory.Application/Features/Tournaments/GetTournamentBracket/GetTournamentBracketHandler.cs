namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;

/// <summary>
/// Handles requests to retrieve a tournament bracket.
/// </summary>
public sealed class GetTournamentBracketHandler(
    IGetTournamentBracketPort port)
{
    public async Task<GetTournamentBracketResponse?> HandleAsync(
        GetTournamentBracketQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await port.GetByTournamentIdAsync(
            new Domain.Identifiers.TournamentId(query.TournamentId),
            cancellationToken);
    }
}
