namespace HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;

/// <summary>
/// Handles requests to update a participation.
/// </summary>
public sealed class UpdateParticipationHandler(
    IUpdateParticipationPort port)
{
    /// <summary>
    /// Handles the request.
    /// </summary>
    public Task<UpdateParticipationResponse?> HandleAsync(
        UpdateParticipationCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        return port.UpdateAsync(
            command,
            cancellationToken);
    }
}