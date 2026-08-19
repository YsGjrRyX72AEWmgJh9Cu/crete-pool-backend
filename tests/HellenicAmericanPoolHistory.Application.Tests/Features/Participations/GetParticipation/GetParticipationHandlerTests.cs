using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Participations.GetParticipation;

public sealed class GetParticipationHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Participation_From_Port()
    {
        var participationId = ParticipationId.New();

        var expected = new GetParticipationResponse(
            participationId.Value,
            Guid.NewGuid(),
            "Get Participation Test Player",
            Guid.NewGuid(),
            "Get Participation Test Tournament",
            new DateOnly(2026, 8, 18),
            3,
            "Registered");

        var port = new TestGetParticipationPort
        {
            Response = expected
        };

        var handler = new GetParticipationHandler(port);

        var query = new GetParticipationQuery(
            participationId);

        var cancellationToken =
            new CancellationTokenSource().Token;

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        Assert.Equal(
            expected,
            result);

        Assert.Equal(
            participationId,
            port.ParticipationId);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Null_When_Participation_Does_Not_Exist()
    {
        var participationId = ParticipationId.New();

        var port = new TestGetParticipationPort();

        var handler = new GetParticipationHandler(port);

        var query = new GetParticipationQuery(
            participationId);

        var result = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Query_Is_Null()
    {
        var port = new TestGetParticipationPort();

        var handler = new GetParticipationHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class TestGetParticipationPort
        : IGetParticipationPort
    {
        public GetParticipationResponse? Response { get; init; }

        public ParticipationId? ParticipationId { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task<GetParticipationResponse?> GetByIdAsync(
            ParticipationId participationId,
            CancellationToken cancellationToken)
        {
            ParticipationId = participationId;
            CancellationToken = cancellationToken;

            return Task.FromResult(Response);
        }
    }
}
