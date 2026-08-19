using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Participations.UpdateParticipation;

public sealed class UpdateParticipationHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Response_From_Port()
    {
        var participationId = ParticipationId.New();

        var command = new UpdateParticipationCommand(
            participationId,
            5,
            ParticipationStatus.CheckedIn);

        var expected = new UpdateParticipationResponse(
            participationId.Value);

        var port = new TestUpdateParticipationPort
        {
            Response = expected
        };

        var handler = new UpdateParticipationHandler(port);

        var cancellationToken =
            new CancellationTokenSource().Token;

        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        Assert.Equal(
            expected,
            result);

        Assert.Same(
            command,
            port.Command);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Null_When_Port_Returns_Null()
    {
        var command = new UpdateParticipationCommand(
            ParticipationId.New(),
            5,
            ParticipationStatus.CheckedIn);

        var port = new TestUpdateParticipationPort();

        var handler = new UpdateParticipationHandler(port);

        var result = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Command_Is_Null()
    {
        var port = new TestUpdateParticipationPort();

        var handler = new UpdateParticipationHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class TestUpdateParticipationPort
        : IUpdateParticipationPort
    {
        public UpdateParticipationResponse? Response { get; init; }

        public UpdateParticipationCommand? Command { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task<UpdateParticipationResponse?> UpdateAsync(
            UpdateParticipationCommand command,
            CancellationToken cancellationToken)
        {
            Command = command;
            CancellationToken = cancellationToken;

            return Task.FromResult(Response);
        }
    }
}
