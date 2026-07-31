namespace HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;

public sealed class DeletePlayerHandler
{
    private readonly IDeletePlayerPort _port;

    public DeletePlayerHandler(IDeletePlayerPort port)
    {
        _port = port;
    }

    public async Task Handle(
        DeletePlayerCommand command,
        CancellationToken cancellationToken)
    {
        await _port.DeleteAsync(command, cancellationToken);
    }
}