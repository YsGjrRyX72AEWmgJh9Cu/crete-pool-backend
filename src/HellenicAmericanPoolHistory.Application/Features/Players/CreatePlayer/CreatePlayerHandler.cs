using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;

/// <summary>
/// Handles the creation of a new player.
/// </summary>
public sealed class CreatePlayerHandler
{
    private readonly ICreatePlayerPort _port;

    public CreatePlayerHandler(ICreatePlayerPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task<PlayerId> HandleAsync(
        CreatePlayerCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var playerId = PlayerId.New();

        var player = new Player(
            playerId,
            command.FirstName,
            command.LastName,
            new Country(command.CountryOfOrigin),
            command.BirthDate);

        return await _port.CreateAsync(player, cancellationToken);
    }
}