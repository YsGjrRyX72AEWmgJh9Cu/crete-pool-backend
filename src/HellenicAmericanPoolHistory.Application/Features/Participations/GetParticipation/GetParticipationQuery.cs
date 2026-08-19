using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;

/// <summary>
/// Represents a request to retrieve a participation by identifier.
/// </summary>
/// <param name="ParticipationId">The participation identifier.</param>
public sealed record GetParticipationQuery(
    ParticipationId ParticipationId);