using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;

/// <summary>
/// Deletes tournaments from persistence.
/// </summary>
public interface IDeleteTournamentPort
{
    Task DeleteAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken);
}