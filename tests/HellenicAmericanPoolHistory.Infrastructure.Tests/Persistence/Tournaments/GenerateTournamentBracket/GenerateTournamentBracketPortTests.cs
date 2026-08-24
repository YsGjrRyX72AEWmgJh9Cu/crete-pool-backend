using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GenerateTournamentBracket;
using Microsoft.EntityFrameworkCore;

using TournamentBracketType =
    HellenicAmericanPoolHistory.Domain.Tournament.BracketType;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.GenerateTournamentBracket;

public sealed class GenerateTournamentBracketPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task GenerateAsync_Should_Create_First_Round_Matches()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4);

        var port = new GenerateTournamentBracketPort(dbContext);

        await port.GenerateAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var matches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id)
            .ToListAsync();

        Assert.Equal(2, matches.Count);

        Assert.All(
            matches,
            match => Assert.Equal(1, match.Round));

        Assert.Equal(
            new[] { 1, 2 },
            matches
                .OrderBy(match => match.BracketPosition)
                .Select(match => match.BracketPosition)
                .ToArray());

        var participantIds = matches
            .SelectMany(match =>
                new[]
                {
                    match.Participant1Id,
                    match.Participant2Id
                })
            .ToHashSet();

        Assert.Equal(
            data.Participants
                .Select(participant => participant.Id)
                .ToHashSet(),
            participantIds);
    }

    [Fact]
    public async Task GenerateAsync_Should_Pair_First_And_Last_Seeds()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4);

        var port = new GenerateTournamentBracketPort(dbContext);

        await port.GenerateAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var matches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id)
            .OrderBy(match => match.BracketPosition)
            .ToListAsync();

        Assert.Equal(2, matches.Count);

        Assert.Equal(
            data.Participants[0].Id,
            matches[0].Participant1Id);

        Assert.Equal(
            data.Participants[3].Id,
            matches[0].Participant2Id);

        Assert.Equal(
            data.Participants[1].Id,
            matches[1].Participant1Id);

        Assert.Equal(
            data.Participants[2].Id,
            matches[1].Participant2Id);
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.GenerateAsync(
                TournamentId.New(),
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_InProgress()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            startTournament: false);

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_Bracket_Type_Is_Not_SingleElimination()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            bracketType: TournamentBracketType.RoundRobin);

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_There_Are_Fewer_Than_Two_Participants()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 1);

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_Participant_Count_Is_Not_Power_Of_Two()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 3);

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_Participant_Has_No_Seed()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            missingSeedIndex: 2);

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_Seeds_Are_Duplicated()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4,
            duplicateSeed: true);

        var port = new GenerateTournamentBracketPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAsync_Should_Create_Bracket_Using_Only_CheckedIn_Participants()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 5,
            nonCheckedInIndex: 4);

        var port = new GenerateTournamentBracketPort(dbContext);

        await port.GenerateAsync(
            data.Tournament.Id,
            CancellationToken.None);

        var matches = await dbContext.Matches
            .Where(match =>
                match.TournamentId == data.Tournament.Id)
            .ToListAsync();

        Assert.Equal(2, matches.Count);

        var checkedInIds = data.Participants
            .Where(participant =>
                participant.Status ==
                ParticipationStatus.CheckedIn)
            .Select(participant => participant.Id)
            .ToHashSet();

        var matchedIds = matches
            .SelectMany(match =>
                new[]
                {
                    match.Participant1Id,
                    match.Participant2Id
                })
            .ToHashSet();

        Assert.Equal(
            checkedInIds,
            matchedIds);
    }

    [Fact]
    public async Task GenerateAsync_Should_Throw_ConflictException_When_Bracket_Already_Exists()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(
            dbContext,
            participantCount: 4);

        var port = new GenerateTournamentBracketPort(dbContext);

        await port.GenerateAsync(
            data.Tournament.Id,
            CancellationToken.None);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.GenerateAsync(
                data.Tournament.Id,
                CancellationToken.None));
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
        int participantCount,
        TournamentBracketType bracketType =
            TournamentBracketType.SingleElimination,
        bool startTournament = true,
        int? missingSeedIndex = null,
        bool duplicateSeed = false,
        int? nonCheckedInIndex = null)
    {
        var venue = Venue.Create(
            $"Generate Bracket Venue {Guid.NewGuid():N}",
            new VenueLocation(
                "Greece",
                "Heraklion",
                $"Generate Bracket Address {Guid.NewGuid():N}"));

        var tournament = Tournament.Create(
            new TournamentData(
                $"Generate Bracket Tournament {Guid.NewGuid():N}",
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
                "Generate Bracket",
                $"Player {index + 1}",
                new Country("Greece"));

            int? seed =
                index == missingSeedIndex
                    ? null
                    : index + 1;

            if (duplicateSeed &&
                index == participantCount - 1)
            {
                seed = 1;
            }

            var participation = Participation.Create(
                player.Id,
                tournament.Id,
                new DateOnly(2026, 8, 18),
                seed);

            if (index != nonCheckedInIndex)
            {
                participation.Update(
                    seed,
                    ParticipationStatus.CheckedIn);
            }

            dbContext.Players.Add(player);
            participants.Add(participation);
        }

        dbContext.Participations.AddRange(participants);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participants);
    }

    private sealed record TestData(
        Tournament Tournament,
        IReadOnlyList<Participation> Participants);
}
