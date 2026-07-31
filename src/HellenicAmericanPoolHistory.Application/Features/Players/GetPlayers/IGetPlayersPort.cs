namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;

public interface IGetPlayersPort
{
    Task<IReadOnlyList<GetPlayersResponse>> GetAsync(
        CancellationToken cancellationToken);
}