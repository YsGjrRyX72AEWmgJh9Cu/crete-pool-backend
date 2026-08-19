namespace HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;

/// <summary>
/// Defines the contract for updating a participation.
/// </summary>
public interface IUpdateParticipationPort
{
    /// <summary>
    /// Updates a participation.
    /// </summary>
    Task<UpdateParticipationResponse?> UpdateAsync(
        UpdateParticipationCommand command,
        CancellationToken cancellationToken);
}