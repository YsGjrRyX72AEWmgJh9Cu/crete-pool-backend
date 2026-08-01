using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;

/// <summary>
/// Defines the persistence contract for creating tournaments.
/// </summary>
public interface ICreateTournamentPort
{
    Task SaveAsync(
        Tournament tournament,
        CancellationToken cancellationToken);
}