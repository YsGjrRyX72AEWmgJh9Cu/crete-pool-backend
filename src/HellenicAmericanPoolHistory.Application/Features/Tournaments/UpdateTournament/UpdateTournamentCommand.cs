using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;

/// <summary>
/// Represents the data required to update an existing tournament.
/// </summary>
public sealed record UpdateTournamentCommand(
    string Name,
    TournamentType TournamentType,
    BracketType BracketType,
    GameSet GameSet,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid VenueId);