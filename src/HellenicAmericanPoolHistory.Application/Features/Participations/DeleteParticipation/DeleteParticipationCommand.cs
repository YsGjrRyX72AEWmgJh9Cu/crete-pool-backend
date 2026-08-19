using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;

public sealed record DeleteParticipationCommand(
    ParticipationId Id);