using HellenicAmericanPoolHistory.Domain.TournamentSeries;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Domain.Tournament;

public sealed record TournamentData(
    string Name,
    TournamentType TournamentType,
    BracketType BracketType,
    GameSet GameSet,
    DateOnly StartDate,
    DateOnly EndDate,
    VenueId VenueId,
    TournamentSeriesId? TournamentSeriesId = null);
