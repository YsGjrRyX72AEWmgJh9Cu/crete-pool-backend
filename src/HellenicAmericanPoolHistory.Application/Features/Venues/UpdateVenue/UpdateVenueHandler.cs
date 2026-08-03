using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;

/// <summary>
/// Handles venue update requests.
/// </summary>
public sealed class UpdateVenueHandler
{
    private readonly IUpdateVenuePort _port;

    public UpdateVenueHandler(IUpdateVenuePort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        UpdateVenueCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var venueId = new VenueId(command.VenueId);

        var data = new VenueData(
            command.Name,
            command.City,
            command.Address);

        await _port.UpdateAsync(
            venueId,
            data,
            cancellationToken);
    }
}