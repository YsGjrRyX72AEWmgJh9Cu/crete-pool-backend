using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;

/// <summary>
/// Represents the tournament returned by the Get Tournament feature.
/// </summary>
public sealed record GetTournamentResponse(
    Guid Id,
    string Name,
    TournamentType TournamentType,
    BracketType BracketType,
    GameSet GameSet,
    TournamentStatus TournamentStatus,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid VenueId);