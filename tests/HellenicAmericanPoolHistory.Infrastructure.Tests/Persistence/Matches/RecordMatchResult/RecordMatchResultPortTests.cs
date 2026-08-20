using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.RecordMatchResult;
using Microsoft.EntityFrameworkCore;

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

        var port = new RecordMatchResultPort(dbContext);

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

        var port = new RecordMatchResultPort(dbContext);

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

        var port = new RecordMatchResultPort(dbContext);

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

        var port = new RecordMatchResultPort(dbContext);

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

        var port = new RecordMatchResultPort(dbContext);

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

        var port = new RecordMatchResultPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.RecordAsync(
                data.Match.Id,
                data.Participant1.Id,
                3,
                5,
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

        var match = new Match(
            MatchId.New(),
            tournament.Id,
            participant1.Id,
            participant2.Id);

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(
            player1,
            player2);
        dbContext.Participations.AddRange(
            participant1,
            participant2);
        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        return new TestData(
            match,
            participant1,
            participant2);
    }

    private sealed record TestData(
        Match Match,
        Participation Participant1,
        Participation Participant2);
}
