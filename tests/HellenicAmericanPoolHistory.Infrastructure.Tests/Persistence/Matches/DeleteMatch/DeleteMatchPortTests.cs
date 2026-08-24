using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.DeleteMatch;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.Persistence.Matches.DeleteMatch;

public sealed class DeleteMatchPortTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=HellenicAmericanPoolHistory_Test;Username=manos";

    [Fact]
    public async Task DeleteAsync_Should_Delete_Existing_Match()
    {
        await using var dbContext = CreateDbContext();

        var data = await CreateTestDataAsync(dbContext);

        var match = new Match(
            MatchId.New(),
            data.Tournament.Id,
            1,
            1,
            data.Participant1.Id,
            data.Participant2.Id);

        match.RecordResult(
            data.Participant1.Id,
            5,
            3);

        dbContext.Matches.Add(match);

        await dbContext.SaveChangesAsync();

        var port = new DeleteMatchPort(dbContext);

        await port.DeleteAsync(
            new DeleteMatchCommand(match.Id),
            CancellationToken.None);

        var persistedMatch = await dbContext.Matches
            .AsNoTracking()
            .SingleOrDefaultAsync(
                item => item.Id == match.Id);

        Assert.Null(persistedMatch);
    }

    [Fact]
    public async Task DeleteAsync_When_Match_Does_Not_Exist_Should_Throw_NotFoundException()
    {
        await using var dbContext = CreateDbContext();

        var port = new DeleteMatchPort(dbContext);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => port.DeleteAsync(
                new DeleteMatchCommand(MatchId.New()),
                CancellationToken.None));

        Assert.Equal(
            "Match not found.",
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
        ApplicationDbContext dbContext)
    {
        var venue = Venue.Create(
            "Delete Match Test Venue",
            new VenueLocation(
                "Greece",
                "Heraklion",
                "Delete Match Test Address"));

        var tournament = Tournament.Create(
            new TournamentData(
                "Delete Match Test Tournament",
                TournamentType.Individual,
                BracketType.SingleElimination,
                GameSet.RaceTo5,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                venue.Id));

        var player1 = Player.Create(
            "Delete Match",
            "Test Player One",
            new Country("Greece"));

        var player2 = Player.Create(
            "Delete Match",
            "Test Player Two",
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

        dbContext.Venues.Add(venue);
        dbContext.Tournaments.Add(tournament);
        dbContext.Players.AddRange(
            player1,
            player2);

        dbContext.Participations.AddRange(
            participant1,
            participant2);

        await dbContext.SaveChangesAsync();

        return new TestData(
            tournament,
            participant1,
            participant2);
    }

    private sealed record TestData(
        Tournament Tournament,
        Participation Participant1,
        Participation Participant2);
}
