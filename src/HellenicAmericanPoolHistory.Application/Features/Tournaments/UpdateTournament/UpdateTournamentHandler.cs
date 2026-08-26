using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;

/// <summary>
/// Handles tournament updates.
/// </summary>
public sealed class UpdateTournamentHandler
{
    private readonly IUpdateTournamentPort _port;

    public UpdateTournamentHandler(IUpdateTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task HandleAsync(
        TournamentId tournamentId,
        UpdateTournamentCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tournamentData = new TournamentData(
            command.Name,
            command.TournamentType,
            command.BracketType,
            command.GameSet,
            command.StartDate,
            command.EndDate,
            new VenueId(command.VenueId),
            null);

        await _port.UpdateAsync(
            tournamentId,
            tournamentData,
            cancellationToken);
    }
}
