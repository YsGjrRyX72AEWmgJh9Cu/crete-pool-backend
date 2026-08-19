using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;

/// <summary>
/// Represents a request to update a participation.
/// </summary>
/// <param name="ParticipationId">The participation identifier.</param>
/// <param name="Seed">The player's seed.</param>
/// <param name="Status">The participation status.</param>
public sealed record UpdateParticipationCommand(
    ParticipationId ParticipationId,
    int? Seed,
    ParticipationStatus Status);