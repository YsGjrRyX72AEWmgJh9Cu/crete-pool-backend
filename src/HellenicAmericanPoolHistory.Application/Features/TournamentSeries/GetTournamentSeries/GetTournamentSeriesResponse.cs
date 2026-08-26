namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;

/// <summary>
/// Represents a tournament series returned by the Get Tournament Series feature.
/// </summary>
public sealed record GetTournamentSeriesResponse(
    Guid Id,
    string Name,
    Guid OrganizationId);
