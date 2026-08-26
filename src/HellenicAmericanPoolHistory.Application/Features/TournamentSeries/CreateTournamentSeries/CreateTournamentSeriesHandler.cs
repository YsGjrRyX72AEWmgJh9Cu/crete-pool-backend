using HellenicAmericanPoolHistory.Domain.Organization;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;

namespace HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;

/// <summary>
/// Handles the creation of a new tournament series.
/// </summary>
public sealed class CreateTournamentSeriesHandler
{
    private readonly ICreateTournamentSeriesPort _port;

    public CreateTournamentSeriesHandler(
        ICreateTournamentSeriesPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task<CreateTournamentSeriesResponse> HandleAsync(
        CreateTournamentSeriesCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tournamentSeries = TournamentSeriesEntity.Create(
            new OrganizationId(command.OrganizationId),
            command.Name);

        await _port.SaveAsync(
            tournamentSeries,
            cancellationToken);

        return new CreateTournamentSeriesResponse(
            tournamentSeries.Id.Value);
    }
}
