namespace HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;

public sealed class DeleteMatchHandler
{
    private readonly IDeleteMatchPort _port;

    public DeleteMatchHandler(
        IDeleteMatchPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        DeleteMatchCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        await _port.DeleteAsync(
            command,
            cancellationToken);
    }
}
