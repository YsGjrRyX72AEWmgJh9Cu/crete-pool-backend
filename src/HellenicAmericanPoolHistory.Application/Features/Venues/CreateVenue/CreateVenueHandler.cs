using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;

/// <summary>
/// Handles the creation of a new venue.
/// </summary>
public sealed class CreateVenueHandler
{
    private readonly ICreateVenuePort _port;

    public CreateVenueHandler(ICreateVenuePort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task<CreateVenueResponse> HandleAsync(
        CreateVenueCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var location = new VenueLocation(
            command.Country,
            command.City,
            command.Address);

        var venue = Venue.Create(
            command.Name,
            location);

        await _port.SaveAsync(
            venue,
            cancellationToken);

        return new CreateVenueResponse(
            venue.Id.Value);
    }
}