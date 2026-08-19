namespace HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;

public sealed class DeleteParticipationHandler
{
    private readonly IDeleteParticipationPort _port;

    public DeleteParticipationHandler(
        IDeleteParticipationPort port)
    {
        _port = port;
    }

    public async Task Handle(
        DeleteParticipationCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        await _port.DeleteAsync(
            command,
            cancellationToken);
    }
}