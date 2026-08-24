using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.CreateMatch;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Matches.CreateMatch;

public sealed class CreateMatchPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task CreateAsync_With_Valid_Match_Should_Persist_Match_Without_Result()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: true);

        var match = new Match(
            MatchId.New(),
            data.Tournament.Id,
            1,
            1,
            data.Participant1.Id,
            data.Participant2.Id);

        var port = new CreateMatchPort(dbContext);

        var result = await port.CreateAsync(match);

        Assert.Equal(match.Id, result);

        var persistedMatch = await dbContext.Matches
            .SingleAsync(x => x.Id == match.Id);

        Assert.Equal(match.TournamentId, persistedMatch.TournamentId);
        Assert.Equal(match.Participant1Id, persistedMatch.Participant1Id);
        Assert.Equal(match.Participant2Id, persistedMatch.Participant2Id);
        Assert.Null(persistedMatch.WinnerParticipationId);
        Assert.Null(persistedMatch.Participant1Score);
        Assert.Null(persistedMatch.Participant2Score);
    }

    [Fact]
    public async Task CreateAsync_When_Tournament_Is_Not_InProgress_Should_Throw_ConflictException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: false);

        var match = new Match(
            MatchId.New(),
            data.Tournament.Id,
            1,
            1,
            data.Participant1.Id,
            data.Participant2.Id);

        var port = new CreateMatchPort(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.CreateAsync(match));

        Assert.Equal(
            "Match can only be created while the tournament is in progress.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_When_Tournament_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: true);

        var missingTournamentId = TournamentId.New();

        var match = new Match(
            MatchId.New(),
            missingTournamentId,
            1,
            1,
            data.Participant1.Id,
            data.Participant2.Id);

        var port = new CreateMatchPort(dbContext);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.CreateAsync(match));

        Assert.Equal(
            "Tournament not found.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_When_Participation_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: true);

        var missingParticipationId = ParticipationId.New();

        var match = new Match(
            MatchId.New(),
            data.Tournament.Id,
            1,
            1,
            data.Participant1.Id,
            missingParticipationId);

        var port = new CreateMatchPort(dbContext);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.CreateAsync(match));

        Assert.Equal(
            "One or more match participations were not found.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_When_Participant_Belongs_To_Another_Tournament_Should_Throw_ConflictException()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: true);

        var match = new Match(
            MatchId.New(),
            data.Tournament.Id,
            1,
            1,
            data.Participant1.Id,
            data.OtherTournamentParticipant.Id);

        var port = new CreateMatchPort(dbContext);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => port.CreateAsync(match));

        Assert.Equal(
            "Participant 2 does not belong to the specified tournament.",
            exception.Message);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext,
        bool startTournament)
    {
        var venue = Venue.Create(
            $"Infrastructure Test Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Test Address {Guid.NewGuid():N}"));

        var otherVenue = Venue.Create(
            $"Infrastructure Test Other Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Test Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Infrastructure Test Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                venue.Id));

        var otherTournament = Tournament.Create(
            new TournamentData(
                $"Infrastructure Test Other Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                otherVenue.Id));

        if (startTournament)
        {
            tournament.Schedule();
            tournament.Start();
        }

        var player1 = Player.Create(
            "Test",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Test",
            "Player Two",
            new Country("Greece"));

        var player3 = Player.Create(
            "Test",
            "Player Three",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            DateOnly.FromDateTime(DateTime.UtcNow),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            DateOnly.FromDateTime(DateTime.UtcNow),
            2);

        var otherTournamentParticipant = Participation.Create(
            player3.Id,
            otherTournament.Id,
            DateOnly.FromDateTime(DateTime.UtcNow),
            1);

        dbContext.Venues.AddRange(
            venue,
            otherVenue);

        dbContext.Tournaments.AddRange(
            tournament,
            otherTournament);

        dbContext.Players.AddRange(
            player1,
            player2,
            player3);

        dbContext.Participations.AddRange(
            participant1,
            participant2,
            otherTournamentParticipant);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2,
            otherTournamentParticipant);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2,
        Participation OtherTournamentParticipant);
}
