using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Venues.GetVenues;

public sealed class GetVenuesHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Venues()
    {
        var expectedResponses =
            new List<GetVenuesResponse>
            {
                new(
                    Guid.NewGuid(),
                    "Venue A",
                    "Greece",
                    "Heraklion",
                    "Address A"),
                new(
                    Guid.NewGuid(),
                    "Venue B",
                    "Greece",
                    "Chania",
                    "Address B")
            };

        var port = new FakeGetVenuesPort(
            expectedResponses);

        var handler = new GetVenuesHandler(port);

        var response = await handler.HandleAsync(
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(
            expectedResponses,
            response);
    }

    [Fact]
    public async Task HandleAsync_When_No_Venues_Exist_Should_Return_Empty_List()
    {
        var port = new FakeGetVenuesPort(
            Array.Empty<GetVenuesResponse>());

        var handler = new GetVenuesHandler(port);

        var response = await handler.HandleAsync(
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Empty(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Call_Port()
    {
        var port = new FakeGetVenuesPort(
            Array.Empty<GetVenuesResponse>());

        var handler = new GetVenuesHandler(port);

        await handler.HandleAsync(
            CancellationToken.None);

        Assert.True(port.WasCalled);
    }

    private sealed class FakeGetVenuesPort
        : IGetVenuesPort
    {
        private readonly IReadOnlyList<GetVenuesResponse> _responses;

        public FakeGetVenuesPort(
            IReadOnlyList<GetVenuesResponse> responses)
        {
            _responses = responses;
        }

        public bool WasCalled { get; private set; }

        public Task<IReadOnlyList<GetVenuesResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            WasCalled = true;

            return Task.FromResult(_responses);
        }
    }
}
