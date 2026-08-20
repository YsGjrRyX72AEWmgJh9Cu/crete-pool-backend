using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;

public sealed class RecordMatchResultHandler
{
    private readonly IRecordMatchResultPort _port;

    public RecordMatchResultHandler(
        IRecordMatchResultPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task HandleAsync(
        RecordMatchResultCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        await _port.RecordAsync(
            new MatchId(command.MatchId),
            new ParticipationId(command.WinnerParticipationId),
            command.Participant1Score,
            command.Participant2Score,
            cancellationToken);
    }
}
