namespace HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;

public interface IDeletePlayerPort
{
    Task DeleteAsync(
        DeletePlayerCommand command,
        CancellationToken cancellationToken);
}