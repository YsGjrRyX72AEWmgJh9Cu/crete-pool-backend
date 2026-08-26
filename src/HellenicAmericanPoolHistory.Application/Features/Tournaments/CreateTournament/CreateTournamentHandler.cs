using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;

/// <summary>
/// Handles the creation of a new tournament.
/// </summary>
public sealed class CreateTournamentHandler
{
    private readonly ICreateTournamentPort _port;

    public CreateTournamentHandler(ICreateTournamentPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task<CreateTournamentResponse> HandleAsync(
        CreateTournamentCommand command,
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

        var tournament = Tournament.Create(tournamentData);

        await _port.SaveAsync(
            tournament,
            cancellationToken);

        return new CreateTournamentResponse(tournament.Id.Value);
    }
}
