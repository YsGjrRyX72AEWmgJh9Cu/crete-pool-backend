namespace HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;

/// <summary>
/// Represents the request to create a new participation.
/// </summary>
/// <param name="PlayerId">The player identifier.</param>
/// <param name="TournamentId">The tournament identifier.</param>
/// <param name="RegistrationDate">The registration date.</param>
/// <param name="Seed">The player's seed.</param>
public sealed record CreateParticipationCommand(
    Guid PlayerId,
    Guid TournamentId,
    DateOnly RegistrationDate,
    int? Seed);