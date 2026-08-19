using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;

/// <summary>
/// Handles the creation of a new participation.
/// </summary>
public sealed class CreateParticipationHandler
{
    private readonly ICreateParticipationPort _port;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateParticipationHandler"/> class.
    /// </summary>
    /// <param name="port">The participation persistence port.</param>
    public CreateParticipationHandler(ICreateParticipationPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    /// <summary>
    /// Handles the creation of a participation.
    /// </summary>
    /// <param name="command">The create participation command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created participation response.</returns>
    public async Task<CreateParticipationResponse> HandleAsync(
        CreateParticipationCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var participation = Participation.Create(
            new PlayerId(command.PlayerId),
            new TournamentId(command.TournamentId),
            command.RegistrationDate,
            command.Seed);

        var createdParticipationId =
            await _port.CreateAsync(
                participation,
                cancellationToken);

        return new CreateParticipationResponse(
            createdParticipationId.Value);
    }
}