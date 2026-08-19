using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.UpdateTournament;

public sealed class UpdateTournamentHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_Port_With_Tournament_Id_And_Data()
    {
        var tournamentId = TournamentId.New();

        var venueId = Guid.NewGuid();

        var command = new UpdateTournamentCommand(
            "Updated Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 8, 14),
            new DateOnly(2026, 8, 15),
            venueId);

        var port = new FakeUpdateTournamentPort();

        var handler = new UpdateTournamentHandler(port);

        await handler.Handle(
            tournamentId.Value,
            command,
            CancellationToken.None);

        Assert.Equal(
            tournamentId,
            port.RequestedTournamentId);

        Assert.NotNull(port.RequestedData);

        Assert.Equal(
            command.Name,
            port.RequestedData.Name);

        Assert.Equal(
            command.TournamentType,
            port.RequestedData.TournamentType);

        Assert.Equal(
            command.BracketType,
            port.RequestedData.BracketType);

        Assert.Equal(
            command.GameSet,
            port.RequestedData.GameSet);

        Assert.Equal(
            command.StartDate,
            port.RequestedData.StartDate);

        Assert.Equal(
            command.EndDate,
            port.RequestedData.EndDate);

        Assert.Equal(
            new VenueId(command.VenueId),
            port.RequestedData.VenueId);
    }

    [Fact]
    public async Task Handle_Should_Complete_When_Port_Succeeds()
    {
        var port = new FakeUpdateTournamentPort();

        var handler = new UpdateTournamentHandler(port);

        var command = CreateCommand();

        var exception = await Record.ExceptionAsync(
            () => handler.Handle(
                TournamentId.New().Value,
                command,
                CancellationToken.None));

        Assert.Null(exception);
        Assert.True(port.WasCalled);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Port_Exception()
    {
        var expectedException =
            new InvalidOperationException("Test exception.");

        var port = new FakeUpdateTournamentPort(
            expectedException);

        var handler = new UpdateTournamentHandler(port);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(
                TournamentId.New().Value,
                CreateCommand(),
                CancellationToken.None));

        Assert.Same(
            expectedException,
            exception);
    }

    private static UpdateTournamentCommand CreateCommand()
    {
        return new UpdateTournamentCommand(
            "Updated Tournament",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo5,
            new DateOnly(2026, 8, 14),
            new DateOnly(2026, 8, 14),
            Guid.NewGuid());
    }

    private sealed class FakeUpdateTournamentPort
        : IUpdateTournamentPort
    {
        private readonly Exception? _exception;

        public FakeUpdateTournamentPort(
            Exception? exception = null)
        {
            _exception = exception;
        }

        public TournamentId? RequestedTournamentId { get; private set; }

        public TournamentData? RequestedData { get; private set; }

        public bool WasCalled { get; private set; }

        public Task UpdateAsync(
            TournamentId tournamentId,
            TournamentData data,
            CancellationToken cancellationToken)
        {
            WasCalled = true;
            RequestedTournamentId = tournamentId;
            RequestedData = data;

            if (_exception is not null)
            {
                return Task.FromException(_exception);
            }

            return Task.CompletedTask;
        }
    }
}
