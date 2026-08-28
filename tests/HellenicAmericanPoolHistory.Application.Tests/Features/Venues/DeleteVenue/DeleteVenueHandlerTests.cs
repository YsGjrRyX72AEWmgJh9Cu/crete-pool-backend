using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Venues.DeleteVenue;

public sealed class DeleteVenueHandlerTests
{
    [Fact]
    public async Task Handle_Should_Pass_VenueId_To_Port()
    {
        var venueId = Guid.NewGuid();

        var command = new DeleteVenueCommand(
            venueId);

        var port = new FakeDeleteVenuePort();

        var handler = new DeleteVenueHandler(port);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Equal(
            venueId,
            port.VenueId.Value);
    }

    [Fact]
    public async Task Handle_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var port = new FakeDeleteVenuePort();

        var handler = new DeleteVenueHandler(port);

        await handler.Handle(
            new DeleteVenueCommand(Guid.NewGuid()),
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeDeleteVenuePort();

        var handler = new DeleteVenueHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.Handle(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeDeleteVenuePort
        : IDeleteVenuePort
    {
        public VenueId VenueId { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task DeleteAsync(
            VenueId venueId,
            CancellationToken cancellationToken)
        {
            VenueId = venueId;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
