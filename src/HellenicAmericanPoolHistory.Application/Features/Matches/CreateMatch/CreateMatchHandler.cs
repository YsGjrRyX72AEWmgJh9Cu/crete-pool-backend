using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

/// <summary>
/// Handles the creation of a new match.
/// </summary>
public sealed class CreateMatchHandler
{
    private readonly ICreateMatchPort _port;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CreateMatchHandler"/> class.
    /// </summary>
    /// <param name="port">The match persistence port.</param>
    public CreateMatchHandler(ICreateMatchPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    /// <summary>
    /// Handles the creation of a match.
    /// </summary>
    /// <param name="command">The create match command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created match response.</returns>
    public async Task<CreateMatchResponse> HandleAsync(
        CreateMatchCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var match = new Match(
            MatchId.New(),
            new TournamentId(command.TournamentId),
            command.Round,
            command.BracketPosition,
            new ParticipationId(command.Participant1Id),
            new ParticipationId(command.Participant2Id));

        if (command.WinnerParticipationId.HasValue &&
            command.Participant1Score.HasValue &&
            command.Participant2Score.HasValue)
        {
            match.RecordResult(
                new ParticipationId(command.WinnerParticipationId.Value),
                command.Participant1Score.Value,
                command.Participant2Score.Value);
        }

        var createdMatchId =
            await _port.CreateAsync(
                match,
                cancellationToken);

        return new CreateMatchResponse(
            createdMatchId.Value);
    }
}
