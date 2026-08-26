namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;

/// <summary>
/// Represents the request to create a tournament series.
/// </summary>
public sealed record CreateTournamentSeriesCommand(
    string Name,
    Guid OrganizationId);
