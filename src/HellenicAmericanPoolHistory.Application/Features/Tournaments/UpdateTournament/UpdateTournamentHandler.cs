using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;

/// <summary>
/// Handles tournament update requests.
/// </summary>
public sealed class UpdateTournamentHandler
{
    private readonly IUpdateTournamentPort _port;

    public UpdateTournamentHandler(IUpdateTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        Guid tournamentId,
        UpdateTournamentCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var data = new TournamentData(
            command.Name,
            command.TournamentType,
            command.BracketType,
            command.GameSet,
            command.StartDate,
            command.EndDate,
            new VenueId(command.VenueId));

        await _port.UpdateAsync(
            new TournamentId(tournamentId),
            data,
            cancellationToken);
    }
}