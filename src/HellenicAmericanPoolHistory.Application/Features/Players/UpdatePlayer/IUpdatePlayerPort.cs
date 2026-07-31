namespace HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;

public interface IUpdatePlayerPort
{
    Task UpdateAsync(
        UpdatePlayerCommand command,
        CancellationToken cancellationToken);
}