using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Venues.UpdateVenue;

public sealed class UpdateVenueHandlerTests
{
    [Fact]
    public async Task Handle_Should_Pass_VenueId_And_Data_To_Port()
    {
        var venueId = Guid.NewGuid();

        var command = new UpdateVenueCommand(
            venueId,
            "Updated Venue",
            "Heraklion",
            "Updated Address");

        var port = new FakeUpdateVenuePort();

        var handler = new UpdateVenueHandler(port);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Equal(
            venueId,
            port.VenueId.Value);

        Assert.NotNull(port.Data);

        Assert.Equal(
            command.Name,
            port.Data.Name);

        Assert.Equal(
            command.City,
            port.Data.City);

        Assert.Equal(
            command.Address,
            port.Data.Address);
    }

    [Fact]
    public async Task Handle_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var port = new FakeUpdateVenuePort();

        var handler = new UpdateVenueHandler(port);

        await handler.Handle(
            new UpdateVenueCommand(
                Guid.NewGuid(),
                "Updated Venue",
                "Chania",
                "Updated Address"),
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeUpdateVenuePort();

        var handler = new UpdateVenueHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.Handle(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeUpdateVenuePort
        : IUpdateVenuePort
    {
        public VenueId VenueId { get; private set; }

        public VenueData? Data { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task UpdateAsync(
            VenueId venueId,
            VenueData data,
            CancellationToken cancellationToken)
        {
            VenueId = venueId;
            Data = data;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
