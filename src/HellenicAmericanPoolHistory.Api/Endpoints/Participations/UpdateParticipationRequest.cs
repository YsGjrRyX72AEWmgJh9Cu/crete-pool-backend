using HellenicAmericanPoolHistory.Domain.Enums;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

public sealed record UpdateParticipationRequest(
    int? Seed,
    ParticipationStatus Status);
