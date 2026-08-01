using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;

/// <summary>
/// Request used to update a tournament.
/// </summary>
public sealed record UpdateTournamentRequest(
    string Name,
    TournamentType TournamentType,
    BracketType BracketType,
    GameSet GameSet,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid VenueId);