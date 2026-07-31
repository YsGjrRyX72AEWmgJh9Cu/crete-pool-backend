namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;

public sealed class GetPlayersHandler
{
    private readonly IGetPlayersPort _port;

    public GetPlayersHandler(IGetPlayersPort port)
    {
        _port = port;
    }

    public Task<IReadOnlyList<GetPlayersResponse>> HandleAsync(
        GetPlayersQuery query,
        CancellationToken cancellationToken)
    {
        return _port.GetAsync(cancellationToken);
    }
}