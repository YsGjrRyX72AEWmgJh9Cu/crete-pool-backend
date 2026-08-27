using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Venues.GetVenue;

public sealed class GetVenueHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Query_Should_Return_Venue()
    {
        var venueId = Guid.NewGuid();

        var expectedResponse = new GetVenueResponse(
            venueId,
            "Test Venue",
            "Greece",
            "Heraklion",
            "Test Address");

        var port = new FakeGetVenuePort(
            expectedResponse);

        var handler = new GetVenueHandler(port);

        var query = new GetVenueQuery(
            venueId);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(
            expectedResponse,
            response);
        Assert.Equal(
            venueId,
            response.Id);
    }

    [Fact]
    public async Task HandleAsync_When_Venue_Does_Not_Exist_Should_Return_Null()
    {
        var port = new FakeGetVenuePort(null);

        var handler = new GetVenueHandler(port);

        var venueId = Guid.NewGuid();

        var query = new GetVenueQuery(
            venueId);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_Venue_Id_To_Port()
    {
        var venueId = Guid.NewGuid();

        var port = new FakeGetVenuePort(null);

        var handler = new GetVenueHandler(port);

        await handler.HandleAsync(
            new GetVenueQuery(venueId),
            CancellationToken.None);

        Assert.Equal(
            venueId,
            port.RequestedVenueId);
    }

    private sealed class FakeGetVenuePort
        : IGetVenuePort
    {
        private readonly GetVenueResponse? _response;

        public FakeGetVenuePort(
            GetVenueResponse? response)
        {
            _response = response;
        }

        public Guid? RequestedVenueId { get; private set; }

        public Task<GetVenueResponse?> GetByIdAsync(
            Guid venueId,
            CancellationToken cancellationToken)
        {
            RequestedVenueId = venueId;

            return Task.FromResult(_response);
        }
    }
}
