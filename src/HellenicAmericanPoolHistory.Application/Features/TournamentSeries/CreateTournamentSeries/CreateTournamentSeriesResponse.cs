namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;

/// <summary>
/// Represents the response after creating a tournament series.
/// </summary>
public sealed record CreateTournamentSeriesResponse(
    Guid TournamentSeriesId);
