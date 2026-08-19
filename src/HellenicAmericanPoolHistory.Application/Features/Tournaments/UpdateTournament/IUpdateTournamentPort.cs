using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;

/// <summary>
/// Updates an existing tournament.
/// </summary>
public interface IUpdateTournamentPort
{
    Task UpdateAsync(
        TournamentId tournamentId,
        TournamentData data,
        CancellationToken cancellationToken);
}