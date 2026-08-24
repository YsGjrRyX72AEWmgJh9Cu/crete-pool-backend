using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Matches.CreateMatch;

public sealed class CreateMatchHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Command_Should_Create_Match()
    {
        var port = new FakeCreateMatchPort();
        var handler = new CreateMatchHandler(port);

        var tournamentId = Guid.NewGuid();
        var participant1Id = Guid.NewGuid();
        var participant2Id = Guid.NewGuid();

        var command = new CreateMatchCommand(
            tournamentId,
            1,
            1,
            participant1Id,
            participant2Id,
            participant1Id,
            5,
            3);

        var response = await handler.HandleAsync(command);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.NotNull(port.CreatedMatch);

        Assert.Equal(
            tournamentId,
            port.CreatedMatch!.TournamentId.Value);

        Assert.Equal(
            1,
            port.CreatedMatch.Round);

        Assert.Equal(
            1,
            port.CreatedMatch.BracketPosition);

        Assert.Equal(
            participant1Id,
            port.CreatedMatch.Participant1Id.Value);

        Assert.Equal(
            participant2Id,
            port.CreatedMatch.Participant2Id.Value);

        Assert.Equal(
            participant1Id,
            port.CreatedMatch.WinnerParticipationId?.Value);

        Assert.Equal(5, port.CreatedMatch.Participant1Score);
        Assert.Equal(3, port.CreatedMatch.Participant2Score);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_The_Id_From_Port()
    {
        var expectedId = MatchId.New();

        var tournamentId = Guid.NewGuid();
        var participant1Id = Guid.NewGuid();
        var participant2Id = Guid.NewGuid();

        var port = new FakeCreateMatchPort(expectedId);
        var handler = new CreateMatchHandler(port);

        var command = new CreateMatchCommand(
            tournamentId,
            1,
            1,
            participant1Id,
            participant2Id,
            participant1Id,
            5,
            3);

        var response = await handler.HandleAsync(command);

        Assert.Equal(expectedId.Value, response.Id);
    }

    [Fact]
    public async Task HandleAsync_Without_Result_Should_Create_Match_Without_Result()
    {
        var port = new FakeCreateMatchPort();
        var handler = new CreateMatchHandler(port);

        var tournamentId = Guid.NewGuid();
        var participant1Id = Guid.NewGuid();
        var participant2Id = Guid.NewGuid();

        var command = new CreateMatchCommand(
            tournamentId,
            1,
            1,
            participant1Id,
            participant2Id,
            null,
            null,
            null);

        var response = await handler.HandleAsync(command);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.NotNull(port.CreatedMatch);

        Assert.Equal(
            tournamentId,
            port.CreatedMatch!.TournamentId.Value);

        Assert.Equal(
            1,
            port.CreatedMatch.Round);

        Assert.Equal(
            1,
            port.CreatedMatch.BracketPosition);

        Assert.Equal(
            participant1Id,
            port.CreatedMatch.Participant1Id.Value);

        Assert.Equal(
            participant2Id,
            port.CreatedMatch.Participant2Id.Value);

        Assert.Null(port.CreatedMatch.WinnerParticipationId);
        Assert.Null(port.CreatedMatch.Participant1Score);
        Assert.Null(port.CreatedMatch.Participant2Score);
    }

    private sealed class FakeCreateMatchPort : ICreateMatchPort
    {
        private readonly MatchId _id;

        public FakeCreateMatchPort()
            : this(MatchId.New())
        {
        }

        public FakeCreateMatchPort(MatchId id)
        {
            _id = id;
        }

        public Match? CreatedMatch { get; private set; }

        public Task<MatchId> CreateAsync(
            Match match,
            CancellationToken cancellationToken = default)
        {
            CreatedMatch = match;

            return Task.FromResult(_id);
        }
    }
}
