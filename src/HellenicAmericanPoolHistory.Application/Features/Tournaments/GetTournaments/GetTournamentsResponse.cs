using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;

/// <summary>
/// Represents a tournament returned by the Get Tournaments feature.
/// </summary>
public sealed record GetTournamentsResponse(
    Guid Id,
    string Name,
    TournamentType TournamentType,
    BracketType BracketType,
    GameSet GameSet,
    TournamentStatus TournamentStatus,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid VenueId);