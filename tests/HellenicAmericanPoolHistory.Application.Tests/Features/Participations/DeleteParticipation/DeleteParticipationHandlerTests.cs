using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Participations.DeleteParticipation;

public sealed class DeleteParticipationHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_Port()
    {
        var participationId = ParticipationId.New();

        var command = new DeleteParticipationCommand(
            participationId);

        var port = new TestDeleteParticipationPort();

        var handler = new DeleteParticipationHandler(port);

        var cancellationToken = new CancellationTokenSource().Token;

        await handler.Handle(
            command,
            cancellationToken);

        Assert.True(port.WasCalled);
        Assert.Equal(
            participationId,
            port.Command!.Id);
        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Port_Exception()
    {
        var command = new DeleteParticipationCommand(
            ParticipationId.New());

        var port = new TestDeleteParticipationPort
        {
            ExceptionToThrow = new InvalidOperationException(
                "Delete failed.")
        };

        var handler = new DeleteParticipationHandler(port);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Equal(
            "Delete failed.",
            exception.Message);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Command_Is_Null()
    {
        var port = new TestDeleteParticipationPort();

        var handler = new DeleteParticipationHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.Handle(
                null!,
                CancellationToken.None));
    }

    private sealed class TestDeleteParticipationPort
        : IDeleteParticipationPort
    {
        public bool WasCalled { get; private set; }

        public DeleteParticipationCommand? Command { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Exception? ExceptionToThrow { get; init; }

        public Task DeleteAsync(
            DeleteParticipationCommand command,
            CancellationToken cancellationToken)
        {
            WasCalled = true;
            Command = command;
            CancellationToken = cancellationToken;

            if (ExceptionToThrow is not null)
            {
                throw ExceptionToThrow;
            }

            return Task.CompletedTask;
        }
    }
}
