using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Venues.CreateVenue;

public sealed class CreateVenueHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Create_Venue_And_Return_Id()
    {
        var port = new FakeCreateVenuePort();

        var handler = new CreateVenueHandler(port);

        var command = new CreateVenueCommand(
            "Test Venue",
            "Greece",
            "Heraklion",
            "Test Address");

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotEqual(
            Guid.Empty,
            response.VenueId);

        Assert.NotNull(
            port.SavedVenue);

        Assert.Equal(
            command.Name,
            port.SavedVenue.Name);

        Assert.Equal(
            command.Country,
            port.SavedVenue.Location.Country);

        Assert.Equal(
            command.City,
            port.SavedVenue.Location.City);

        Assert.Equal(
            command.Address,
            port.SavedVenue.Location.Address);

        Assert.Equal(
            response.VenueId,
            port.SavedVenue.Id.Value);
    }

    [Fact]
    public async Task HandleAsync_Should_Create_Venue_Without_Address()
    {
        var port = new FakeCreateVenuePort();

        var handler = new CreateVenueHandler(port);

        var command = new CreateVenueCommand(
            "Test Venue",
            "Greece",
            "Heraklion",
            null);

        await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotNull(
            port.SavedVenue);

        Assert.Null(
            port.SavedVenue.Location.Address);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var port = new FakeCreateVenuePort();

        var handler = new CreateVenueHandler(port);

        await handler.HandleAsync(
            new CreateVenueCommand(
                "Test Venue",
                "Greece",
                "Heraklion",
                null),
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeCreateVenuePort();

        var handler = new CreateVenueHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeCreateVenuePort
        : ICreateVenuePort
    {
        public Venue? SavedVenue { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task SaveAsync(
            Venue venue,
            CancellationToken cancellationToken)
        {
            SavedVenue = venue;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
