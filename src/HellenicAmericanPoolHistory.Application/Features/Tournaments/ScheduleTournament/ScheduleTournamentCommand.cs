using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;

/// <summary>
/// Represents a request to schedule a tournament.
/// </summary>
public sealed record ScheduleTournamentCommand(
    TournamentId TournamentId);
