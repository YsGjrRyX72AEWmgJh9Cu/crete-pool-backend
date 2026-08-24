using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using ParticipationStatus =
    HellenicAmericanPoolHistory.Domain.Enums.ParticipationStatus;
using TournamentStatus =
    HellenicAmericanPoolHistory.Domain.Tournament.TournamentStatus;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.RecordMatchResult;
using Microsoft.EntityFrameworkCore;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.AdvanceTournamentBracket;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Matches.RecordMatchResult;

public sealed class RecordMatchResultPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task RecordAsync_Should_Persist_Result()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await port.RecordAsync(
            data.Match.Id,
            data.Participant1.Id,
            5,
            3,
            CancellationToken.None);

        await dbContext.Entry(data.Match).ReloadAsync();

        Assert.Equal(
            data.Participant1.Id,
            data.Match.WinnerParticipationId);

        Assert.Equal(
            5,
            data.Match.Participant1Score);

        Assert.Equal(
            3,
            data.Match.Participant2Score);
    }

    [Fact]
    public async Task RecordAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_InProgress()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: false);

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await Assert.ThrowsAsync<ConflictException>(
            () => port.RecordAsync(
                data.Match.Id,
                data.Participant1.Id,
                5,
                3,
                CancellationToken.None));
    }

    [Fact]
    public async Task RecordAsync_Should_Throw_NotFoundException_When_Match_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.RecordAsync(
                MatchId.New(),
                ParticipationId.New(),
                5,
                3,
                CancellationToken.None));
    }

    [Fact]
    public async Task RecordAsync_Should_Throw_ConflictException_When_Result_Already_Exists()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        data.Match.RecordResult(
            data.Participant1.Id,
            5,
            3);

        await dbContext.SaveChangesAsync();

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await Assert.ThrowsAsync<ConflictException>(
            () => port.RecordAsync(
                data.Match.Id,
                data.Participant1.Id,
                6,
                4,
                CancellationToken.None));
    }

    [Fact]
    public async Task RecordAsync_Should_Throw_ConflictException_When_Winner_Is_Not_A_Participant()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await Assert.ThrowsAsync<ConflictException>(
            () => port.RecordAsync(
                data.Match.Id,
                ParticipationId.New(),
                5,
                3,
                CancellationToken.None));
    }

    [Fact]
    public async Task RecordAsync_Should_Throw_ConflictException_When_Winner_Does_Not_Have_Higher_Score()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await Assert.ThrowsAsync<ConflictException>(
            () => port.RecordAsync(
                data.Match.Id,
                data.Participant1.Id,
                3,
                5,
                CancellationToken.None));
    }

    [Fact]
    public async Task RecordAsync_Should_Not_Create_Next_Round_Until_All_Matches_Are_Completed()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new RecordMatchResultPort(
            dbContext,
            new FakeAdvanceTournamentBracketPort());

        await port.RecordAsync(
            data.Match.Id,
            data.Participant1.Id,
            5,
            3,
            CancellationToken.None);

        var nextRoundMatches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 2)
            .ToListAsync();

        Assert.Empty(nextRoundMatches);

        var winner = await dbContext.Participations
            .SingleAsync(participation =>
                participation.Id == data.Participant1.Id);

        var loser = await dbContext.Participations
            .SingleAsync(participation =>
                participation.Id == data.Participant3.Id);

        var tournament = await dbContext.Tournaments
            .SingleAsync(tournament =>
                tournament.Id == data.Tournament.Id);

        Assert.Equal(
            ParticipationStatus.CheckedIn,
            winner.Status);

        Assert.Equal(
            ParticipationStatus.CheckedIn,
            loser.Status);

        Assert.Equal(
            TournamentStatus.InProgress,
            tournament.TournamentStatus);
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
        bool startTournament = true)
    {
        var venue = Venue.Create(
            $"Record Match Result Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Record Match Result Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Record Match Result Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 20),
                new DateOnly(2026, 8, 20),
                venue.Id));

        if (startTournament)
        {
            tournament.Schedule();
            tournament.Start();
        }

        var player1 = Player.Create(
            "Record Match Result",
            "Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Record Match Result",
            "Player Two",
            new Country("Greece"));

        var player3 = Player.Create(
            "Record Match Result",
            "Player Three",
            new Country("Greece"));

        var player4 = Player.Create(
            "Record Match Result",
            "Player Four",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            2);

        participant1.Update(
            participant1.Seed,
            ParticipationStatus.CheckedIn);

        participant2.Update(
            participant2.Seed,
            ParticipationStatus.CheckedIn);

        var participant3 = Participation.Create(
            player3.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            3);

        var participant4 = Participation.Create(
            player4.Id,
            tournament.Id,
            new DateOnly(2026, 8, 18),
            4);

        participant3.Update(
            participant3.Seed,
            ParticipationStatus.CheckedIn);

        participant4.Update(
            participant4.Seed,
            ParticipationStatus.CheckedIn);

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant2.Id);

        var match2 = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            2,
            participant3.Id,
            participant4.Id);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        dbContext.Players.AddRange(
            player1,
            player2,
            player3,
            player4);

        dbContext.Participations.AddRange(
            participant1,
            participant2,
            participant3,
            participant4);

        dbContext.Matches.AddRange(
            match,
            match2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            match,
            match2,
            participant1,
            participant2,
            participant3,
            participant4);
    }

    private sealed record TestData(
        Tournament Tournament,
        Match Match,
        Match Match2,
        Participation Participant1,
        Participation Participant2,
        Participation Participant3,
        Participation Participant4);

    private sealed class FakeAdvanceTournamentBracketPort
        : IAdvanceTournamentBracketPort
    {
        public Task AdvanceAsync(
            TournamentId tournamentId,
            CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}


    [Fact]
    public async Task RecordAsync_Should_Create_Next_Round_When_All_Matches_Are_Completed()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new RecordMatchResultPort(
            dbContext,
            new AdvanceTournamentBracketPort(dbContext));

        await port.RecordAsync(
            data.Match.Id,
            data.Participant1.Id,
            5,
            3,
            CancellationToken.None);

        await port.RecordAsync(
            data.Match2.Id,
            data.Participant3.Id,
            5,
            2,
            CancellationToken.None);

        var nextRoundMatches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 2)
            .ToListAsync();

        Assert.Single(nextRoundMatches);

        var nextRoundMatch = nextRoundMatches[0];

        Assert.Equal(2, nextRoundMatch.Round);
        Assert.Equal(1, nextRoundMatch.BracketPosition);

        Assert.Equal(
            data.Participant1.Id,
            nextRoundMatch.Participant1Id);

        Assert.Equal(
            data.Participant3.Id,
            nextRoundMatch.Participant2Id);
    }

    [Fact]
    public async Task RecordAsync_Should_Not_Create_Next_Round_After_Final()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var port = new RecordMatchResultPort(
            dbContext,
            new AdvanceTournamentBracketPort(dbContext));

        await port.RecordAsync(
            data.Match.Id,
            data.Participant1.Id,
            5,
            3,
            CancellationToken.None);

        await port.RecordAsync(
            data.Match2.Id,
            data.Participant3.Id,
            5,
            2,
            CancellationToken.None);

        var final = await dbContext.Matches
            .SingleAsync(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 2);

        await port.RecordAsync(
            final.Id,
            data.Participant1.Id,
            5,
            4,
            CancellationToken.None);

        var nextRoundMatches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 3)
            .ToListAsync();

        Assert.Empty(nextRoundMatches);
    }
}
