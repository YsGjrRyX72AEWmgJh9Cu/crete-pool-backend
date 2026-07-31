namespace HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;

public sealed class UpdatePlayerHandler
{
    private readonly IUpdatePlayerPort _port;

    public UpdatePlayerHandler(IUpdatePlayerPort port)
    {
        _port = port;
    }

    public async Task Handle(
        UpdatePlayerCommand command,
        CancellationToken cancellationToken)
    {
        await _port.UpdateAsync(command, cancellationToken);
    }
}