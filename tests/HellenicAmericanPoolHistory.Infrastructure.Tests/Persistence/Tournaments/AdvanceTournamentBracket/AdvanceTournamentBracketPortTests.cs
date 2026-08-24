using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.AdvanceTournamentBracket;
using Microsoft.EntityFrameworkCore;

using TournamentBracketType =
    HellenicAmericanPoolHistory.Domain.Tournament.BracketType;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.AdvanceTournamentBracket;

public sealed class AdvanceTournamentBracketPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task AdvanceAsync_Should_Create_Next_Round_When_Current_Round_Is_Complete()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        data.Match2!.RecordResult(
            data.Participant3.Id,
            5,
            2);

        await dbContext.SaveChangesAsync();

        var port = new AdvanceTournamentBracketPort(dbContext);

        await port.AdvanceAsync(
            data.Tournament.Id,
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
    public async Task AdvanceAsync_Should_Throw_ConflictException_When_Current_Round_Is_Not_Complete()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        await dbContext.SaveChangesAsync();

        var port = new AdvanceTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.AdvanceAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task AdvanceAsync_Should_Not_Create_Next_Round_After_Final()
    {
        await using var dbContext = CreateDbContext(
            participantCount: 2);

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 2);

        Assert.NotNull(data.Match1);
        Assert.Null(data.Match2);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        await dbContext.SaveChangesAsync();

        var port = new AdvanceTournamentBracketPort(dbContext);

        await port.AdvanceAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var matches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id)
            .ToListAsync();

        Assert.Single(matches);

        Assert.Equal(1, matches[0].Round);
    }

    [Fact]
    public async Task AdvanceAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new AdvanceTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.AdvanceAsync(
                TournamentId.New(),
                CancellationToken.None));
    }

    [Fact]
    public async Task AdvanceAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_InProgress()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            startTournament: false);

        var port = new AdvanceTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.AdvanceAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task AdvanceAsync_Should_Throw_ConflictException_When_Bracket_Type_Is_Not_SingleElimination()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            bracketType: TournamentBracketType.RoundRobin);

        var port = new AdvanceTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.AdvanceAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task AdvanceAsync_Should_Throw_ConflictException_When_Bracket_Has_Not_Been_Generated()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            createMatches: false);

        var port = new AdvanceTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.AdvanceAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task AdvanceAsync_Should_Not_Create_Duplicate_Next_Round()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        data.Match2!.RecordResult(
            data.Participant3.Id,
            5,
            2);

        await dbContext.SaveChangesAsync();

        var port = new AdvanceTournamentBracketPort(dbContext);

        await port.AdvanceAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var nextRoundMatchesBeforeSecondAdvance =
            await dbContext.Matches
                .Where(match =>
                    match.TournamentId == data.Tournament.Id &&
                    match.Round == 2)
                .ToListAsync();

        Assert.Single(nextRoundMatchesBeforeSecondAdvance);

        var nextRoundMatch =
            nextRoundMatchesBeforeSecondAdvance[0];

        nextRoundMatch.RecordResult(
            data.Participant1.Id,
            5,
            3);

        await dbContext.SaveChangesAsync();

        await port.AdvanceAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var nextRoundMatchesAfterSecondAdvance =
            await dbContext.Matches
                .Where(match =>
                    match.TournamentId == data.Tournament.Id &&
                    match.Round == 2)
                .ToListAsync();

        Assert.Single(nextRoundMatchesAfterSecondAdvance);

        Assert.Equal(
            nextRoundMatch.Id,
            nextRoundMatchesAfterSecondAdvance[0].Id);
    }

    [Fact]
    public async Task AdvanceAsync_Should_Complete_Tournament_After_Final()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        data.Match1!.RecordResult(
            data.Participant1.Id,
            5,
            3);

        data.Match2!.RecordResult(
            data.Participant3.Id,
            5,
            2);

        await dbContext.SaveChangesAsync();

        var port = new AdvanceTournamentBracketPort(dbContext);

        await port.AdvanceAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var final = await dbContext.Matches
            .SingleAsync(match =>
                match.TournamentId == data.Tournament.Id &&
                match.Round == 2);

        Assert.Equal(
            data.Participant1.Id,
            final.Participant1Id);

        Assert.Equal(
            data.Participant3.Id,
            final.Participant2Id);

        final.RecordResult(
            data.Participant1.Id,
            5,
            4);

        await dbContext.SaveChangesAsync();

        await port.AdvanceAsync(
            data.Tournament.Id,
            CancellationToken.None);

        await dbContext.Entry(data.Tournament).ReloadAsync();

        Assert.Equal(
            HellenicAmericanPoolHistory.Domain.Tournament.TournamentStatus.Completed,
            data.Tournament.TournamentStatus);
    }

    private static ApplicationDbContext CreateDbContext(
        int participantCount = 4)
    {
        var options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<TestData> CreateTestDataAsync(
        ApplicationDbContext dbContext,
        int participantCount = 4,
        TournamentBracketType bracketType =
            TournamentBracketType.SingleElimination,
        bool startTournament = true,
        bool createMatches = true)
    {
        var venue = Venue.Create(
            $"Advance Bracket Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Advance Bracket Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Advance Bracket Tournament {Guid.NewGuid():N}",
                TournamentType.Individual,
                bracketType,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 21),
                new DateOnly(2026, 8, 21),
                venue.Id));

        if (startTournament)
        {
            tournament.Schedule();
            tournament.Start();
        }

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        var participants = new List<Participation>();

        for (var index = 0;
             index < participantCount;
             index++)
        {
            var player = Player.Create(
                "Advance Bracket",
                $"Player {index + 1}",
                new Country("Greece"));

            var participant = Participation.Create(
                player.Id,
                tournament.Id,
                new DateOnly(2026, 8, 18),
                index + 1);

            participant.Update(
                index + 1,
                ParticipationStatus.CheckedIn);

            dbContext.Players.Add(player);
            dbContext.Participations.Add(participant);

            participants.Add(participant);
        }

        Match? match1 = null;
        Match? match2 = null;

        if (createMatches)
        {
            if (participants.Count >= 2)
            {
                match1 = new Match(
                    MatchId.New(),
                    tournament.Id,
                    1,
                    1,
                    participants[0].Id,
                    participants[1].Id);

                dbContext.Matches.Add(match1);
            }

            if (participants.Count >= 4)
            {
                match2 = new Match(
                    MatchId.New(),
                    tournament.Id,
                    1,
                    2,
                    participants[2].Id,
                    participants[3].Id);

                dbContext.Matches.Add(match2);
            }
        }

        await dbContext.SaveChangesAsync();

        return new TestData(
        tournament,
        match1,
        match2,
        participants[0],
        participants[1],
        participants.Count > 2
            ? participants[2]
            : participants[0],
        participants.Count > 3
            ? participants[3]
            : participants[1]);
    }

    private sealed record TestData(
        Tournament Tournament,
        Match? Match1,
        Match? Match2,
        Participation Participant1,
        Participation Participant2,
        Participation Participant3,
        Participation Participant4);
}
