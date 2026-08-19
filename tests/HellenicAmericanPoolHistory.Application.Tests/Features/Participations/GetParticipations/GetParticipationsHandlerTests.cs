using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Participations.GetParticipations;

public sealed class GetParticipationsHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Participations_From_Port()
    {
        var expected = new[]
        {
            new GetParticipationsResponse(
                Guid.NewGuid(),
                "Get Participations Player One",
                "Get Participations Tournament",
                new DateOnly(2026, 8, 18),
                1,
                "Registered"),

            new GetParticipationsResponse(
                Guid.NewGuid(),
                "Get Participations Player Two",
                "Get Participations Tournament",
                new DateOnly(2026, 8, 18),
                2,
                "CheckedIn")
        };

        var port = new TestGetParticipationsPort
        {
            Response = expected
        };

        var handler = new GetParticipationsHandler(port);

        var query = new GetParticipationsQuery();

        var cancellationToken =
            new CancellationTokenSource().Token;

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        Assert.Equal(
            expected,
            result);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Empty_Collection_When_Port_Returns_Empty_Collection()
    {
        var port = new TestGetParticipationsPort
        {
            Response = Array.Empty<GetParticipationsResponse>()
        };

        var handler = new GetParticipationsHandler(port);

        var result = await handler.HandleAsync(
            new GetParticipationsQuery(),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Query_Is_Null()
    {
        var port = new TestGetParticipationsPort();

        var handler = new GetParticipationsHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class TestGetParticipationsPort
        : IGetParticipationsPort
    {
        public IReadOnlyCollection<GetParticipationsResponse> Response { get; init; }
            = Array.Empty<GetParticipationsResponse>();

        public CancellationToken CancellationToken { get; private set; }

        public Task<IReadOnlyCollection<GetParticipationsResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;

            return Task.FromResult(Response);
        }
    }
}
