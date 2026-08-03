using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;

/// <summary>
/// Handles venue deletion requests.
/// </summary>
public sealed class DeleteVenueHandler
{
    private readonly IDeleteVenuePort _port;

    public DeleteVenueHandler(IDeleteVenuePort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task Handle(
        DeleteVenueCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var venueId = new VenueId(command.VenueId);

        await _port.DeleteAsync(
            venueId,
            cancellationToken);
    }
}