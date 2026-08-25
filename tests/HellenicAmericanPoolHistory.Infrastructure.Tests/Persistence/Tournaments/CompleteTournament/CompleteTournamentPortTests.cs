using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CompleteTournament;
using Microsoft.EntityFrameworkCore;

using TournamentStatus =
    HellenicAmericanPoolHistory.Domain.Tournament.TournamentStatus;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Tournaments.CompleteTournament;

public sealed class CompleteTournamentPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task CompleteAsync_Should_Complete_InProgress_Tournament_When_Final_Is_Completed()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateInProgressTournamentWithFinalAsync(
            dbContext,
            finalCompleted: true);

        var port = new CompleteTournamentPort(dbContext);

        await port.CompleteAsync(
            data.Tournament.Id,
            CancellationToken.None);

        await dbContext.Entry(data.Tournament).ReloadAsync();

        Assert.Equal(
            TournamentStatus.Completed,
            data.Tournament.TournamentStatus);
    }

    [Fact]
    public async Task CompleteAsync_Should_Throw_ConflictException_When_Final_Is_Not_Completed()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateInProgressTournamentWithFinalAsync(
            dbContext,
            finalCompleted: false);

        var port = new CompleteTournamentPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.CompleteAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task CompleteAsync_Should_Throw_ConflictException_When_Any_Match_Is_Not_Completed()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateInProgressTournamentWithIncompletePreviousMatchAsync(
            dbContext);

        var port = new CompleteTournamentPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.CompleteAsync(
                data.Tournament.Id,
                CancellationToken.None));
    }

    [Fact]
    public async Task CompleteAsync_Should_Throw_NotFoundException_When_Tournament_Does_Not_Exist()
    {
        await using var dbContext = CreateDbContext();

        var port = new CompleteTournamentPort(dbContext);

        await Assert.ThrowsAsync<NotFoundException>(
            () => port.CompleteAsync(
                TournamentId.New(),
                CancellationToken.None));
    }

    [Fact]
    public async Task CompleteAsync_Should_Throw_ConflictException_When_Tournament_Is_Not_InProgress()
    {
        await using var dbContext = CreateDbContext();

        var tournament = await CreateTournamentAsync(dbContext);

        var port = new CompleteTournamentPort(dbContext);

        await Assert.ThrowsAsync<ConflictException>(
            () => port.CompleteAsync(
                tournament.Id,
                CancellationToken.None));
    }

    private static async Task<TestData> CreateInProgressTournamentWithIncompletePreviousMatchAsync(
        ApplicationDbContext dbContext)
    {
        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();
        tournament.Start();

        var player1 = Player.Create(
            "Incomplete Player 1",
            "incomplete1@test.com",
            new Country("Greece"));

        var player2 = Player.Create(
            "Incomplete Player 2",
            "incomplete2@test.com",
            new Country("Greece"));

        var player3 = Player.Create(
            "Incomplete Player 3",
            "incomplete3@test.com",
            new Country("Greece"));

        var player4 = Player.Create(
            "Incomplete Player 4",
            "incomplete4@test.com",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            2);

        var participant3 = Participation.Create(
            player3.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            3);

        var participant4 = Participation.Create(
            player4.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            4);

        participant1.Update(1, ParticipationStatus.CheckedIn);
        participant2.Update(2, ParticipationStatus.CheckedIn);
        participant3.Update(3, ParticipationStatus.CheckedIn);
        participant4.Update(4, ParticipationStatus.CheckedIn);

        var incompleteMatch = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant2.Id);

        var completedFinal = new Match(
            MatchId.New(),
            tournament.Id,
            2,
            1,
            participant3.Id,
            participant4.Id);

        completedFinal.RecordResult(
            participant3.Id,
            5,
            3);

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
            incompleteMatch,
            completedFinal);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            completedFinal,
            participant3,
            participant4);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<Tournament> CreateTournamentAsync(
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            "Complete Tournament Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Complete Tournament Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Complete Tournament Test",
                TournamentType.Individual,
                HellenicAmericanPoolHistory.Domain.Tournament.BracketType.SingleElimination,
                GameSet.RaceTo5,
                new DateOnly(2026, 8, 14),
                new DateOnly(2026, 8, 14),
                venue.Id));

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);

        await dbContext.SaveChangesAsync();

        return tournament;
    }

    private static async Task<TestData> CreateInProgressTournamentWithFinalAsync(
        ApplicationDbContext dbContext,
        bool finalCompleted)
    {
        var tournament = await CreateTournamentAsync(dbContext);

        tournament.Schedule();
        tournament.Start();

        var player1 = Player.Create(
            "Final Player 1",
            "final1@test.com",
            new Country("Greece"));

        var player2 = Player.Create(
            "Final Player 2",
            "final2@test.com",
            new Country("Greece"));

        var participant1 = Participation.Create(
            player1.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            1);

        var participant2 = Participation.Create(
            player2.Id,
            tournament.Id,
            new DateOnly(2026, 8, 14),
            2);

        participant1.Update(
            1,
            ParticipationStatus.CheckedIn);

        participant2.Update(
            2,
            ParticipationStatus.CheckedIn);

        var final = new Match(
            MatchId.New(),
            tournament.Id,
            1,
            1,
            participant1.Id,
            participant2.Id);

        if (finalCompleted)
        {
            final.RecordResult(
                participant1.Id,
                5,
                3);
        }

        dbContext.Players.AddRange(
            player1,
            player2);

        dbContext.Participations.AddRange(
            participant1,
            participant2);

        dbContext.Matches.Add(final);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            final,
            participant1,
            participant2);
    }

    private sealed record TestData(
        Tournament Tournament,
        Match Final,
        Participation Participant1,
        Participation Participant2);
}
