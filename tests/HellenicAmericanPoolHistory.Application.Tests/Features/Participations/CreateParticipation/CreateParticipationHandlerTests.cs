using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Participations.CreateParticipation;

public sealed class CreateParticipationHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Command_Should_Return_Created_Participation()
    {
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();
        var registrationDate = new DateOnly(2026, 8, 18);
        var seed = 3;

        var port = new FakeCreateParticipationPort();
        var handler = new CreateParticipationHandler(port);

        var command = new CreateParticipationCommand(
            playerId.Value,
            tournamentId.Value,
            registrationDate,
            seed);

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.NotNull(port.CreatedParticipation);

        Assert.Equal(
            playerId,
            port.CreatedParticipation.PlayerId);

        Assert.Equal(
            tournamentId,
            port.CreatedParticipation.TournamentId);

        Assert.Equal(
            registrationDate,
            port.CreatedParticipation.RegistrationDate);

        Assert.Equal(
            seed,
            port.CreatedParticipation.Seed);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_Participation_To_Port()
    {
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();

        var port = new FakeCreateParticipationPort();
        var handler = new CreateParticipationHandler(port);

        var command = new CreateParticipationCommand(
            playerId.Value,
            tournamentId.Value,
            new DateOnly(2026, 8, 18),
            2);

        await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotNull(port.CreatedParticipation);

        Assert.Equal(
            playerId,
            port.CreatedParticipation!.PlayerId);

        Assert.Equal(
            tournamentId,
            port.CreatedParticipation.TournamentId);

        Assert.Equal(
            new DateOnly(2026, 8, 18),
            port.CreatedParticipation.RegistrationDate);

        Assert.Equal(
            2,
            port.CreatedParticipation.Seed);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Port_Participation_Id()
    {
        var participationId = ParticipationId.New();

        var port = new FakeCreateParticipationPort(
            participationId);

        var handler = new CreateParticipationHandler(port);

        var command = new CreateParticipationCommand(
            PlayerId.New().Value,
            TournamentId.New().Value,
            new DateOnly(2026, 8, 18),
            null);

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.Equal(
            participationId.Value,
            response.Id);
    }

    [Fact]
    public async Task HandleAsync_With_Null_Command_Should_Throw()
    {
        var port = new FakeCreateParticipationPort();
        var handler = new CreateParticipationHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeCreateParticipationPort
        : ICreateParticipationPort
    {
        private readonly ParticipationId _participationId;

        public FakeCreateParticipationPort()
            : this(ParticipationId.New())
        {
        }

        public FakeCreateParticipationPort(
            ParticipationId participationId)
        {
            _participationId = participationId;
        }

        public Participation? CreatedParticipation { get; private set; }

        public Task<ParticipationId> CreateAsync(
            Participation participation,
            CancellationToken cancellationToken = default)
        {
            CreatedParticipation = participation;

            return Task.FromResult(_participationId);
        }
    }
}
