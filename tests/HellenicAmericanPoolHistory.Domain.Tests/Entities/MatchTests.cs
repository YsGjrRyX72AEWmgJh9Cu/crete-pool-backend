using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

/// <summary>
/// Unit tests for the <see cref="Match"/> entity.
/// </summary>
public class MatchTests
{
    [Fact]
    public void Constructor_With_Valid_Values_Should_Create_Match()
    {
        // Arrange
        var tournamentId = TournamentId.New();
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        // Act
        var match = new Match(
            MatchId.New(),
            tournamentId,
            participant1Id,
            participant2Id,
            participant1Id,
            9,
            5);

        // Assert
        Assert.Equal(tournamentId, match.TournamentId);
        Assert.Equal(participant1Id, match.Participant1Id);
        Assert.Equal(participant2Id, match.Participant2Id);
        Assert.Equal(
            participant1Id,
            match.WinnerParticipationId);
        Assert.Equal(9, match.Participant1Score);
        Assert.Equal(5, match.Participant2Score);
    }

    [Fact]
    public void Constructor_With_Default_TournamentId_Should_Throw()
    {
        // Arrange
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                default,
                participant1Id,
                participant2Id,
                participant1Id,
                9,
                5));
    }

    [Fact]
    public void Constructor_With_Same_Participants_Should_Throw()
    {
        // Arrange
        var tournamentId = TournamentId.New();
        var participantId = ParticipationId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                tournamentId,
                participantId,
                participantId,
                participantId,
                9,
                0));
    }

    [Fact]
    public void Constructor_With_Winner_Not_In_Match_Should_Throw()
    {
        // Arrange
        var tournamentId = TournamentId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                tournamentId,
                ParticipationId.New(),
                ParticipationId.New(),
                ParticipationId.New(),
                9,
                5));
    }

    [Fact]
    public void Constructor_With_Negative_Participant1Score_Should_Throw()
    {
        // Arrange
        var tournamentId = TournamentId.New();
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Match(
                MatchId.New(),
                tournamentId,
                participant1Id,
                participant2Id,
                participant1Id,
                -1,
                5));
    }

    [Fact]
    public void Constructor_With_Negative_Participant2Score_Should_Throw()
    {
        // Arrange
        var tournamentId = TournamentId.New();
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Match(
                MatchId.New(),
                tournamentId,
                participant1Id,
                participant2Id,
                participant1Id,
                5,
                -1));
    }

    [Fact]
    public void Constructor_When_Winner_Does_Not_Have_Higher_Score_Should_Throw()
    {
        // Arrange
        var tournamentId = TournamentId.New();
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                tournamentId,
                participant1Id,
                participant2Id,
                participant1Id,
                5,
                9));
    }
}