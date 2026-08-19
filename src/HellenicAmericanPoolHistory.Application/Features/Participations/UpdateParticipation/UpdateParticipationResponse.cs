namespace HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;

/// <summary>
/// Represents the result of updating a participation.
/// </summary>
/// <param name="Id">The participation identifier.</param>
public sealed record UpdateParticipationResponse(Guid Id);