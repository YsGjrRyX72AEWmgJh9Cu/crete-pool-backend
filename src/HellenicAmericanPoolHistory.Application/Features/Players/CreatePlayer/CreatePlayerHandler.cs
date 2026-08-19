using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;

/// <summary>
/// Handles the creation of a new player.
/// </summary>
public sealed class CreatePlayerHandler
{
    private readonly ICreatePlayerPort _port;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerHandler"/> class.
    /// </summary>
    /// <param name="port">The player persistence port.</param>
    public CreatePlayerHandler(ICreatePlayerPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    /// <summary>
    /// Handles the creation of a player.
    /// </summary>
    /// <param name="command">The create player command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created player response.</returns>
    public async Task<CreatePlayerResponse> HandleAsync(
        CreatePlayerCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var player = Player.Create(
            command.FirstName,
            command.LastName,
            new Country(command.CountryOfOrigin),
            command.BirthDate);

        var createdPlayerId =
            await _port.CreateAsync(
                player,
                cancellationToken);

        return new CreatePlayerResponse(
            createdPlayerId.Value);
    }
}