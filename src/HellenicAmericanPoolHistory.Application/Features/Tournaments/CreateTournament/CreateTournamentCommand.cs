using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;

/// <summary>
/// Represents the request to create a tournament.
/// </summary>
public sealed record CreateTournamentCommand(
    string Name,
    TournamentType TournamentType,
    BracketType BracketType,
    GameSet GameSet,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid VenueId);