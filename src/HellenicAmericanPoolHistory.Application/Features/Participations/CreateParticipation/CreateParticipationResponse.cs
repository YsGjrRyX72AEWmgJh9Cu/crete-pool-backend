namespace HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;

/// <summary>
/// Represents the response after creating a participation.
/// </summary>
/// <param name="Id">The participation identifier.</param>
public sealed record CreateParticipationResponse(Guid Id);