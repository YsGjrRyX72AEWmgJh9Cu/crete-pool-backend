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
            1,
            1,
            participant1Id,
            participant2Id);

        // Assert
        Assert.Equal(tournamentId, match.TournamentId);
        Assert.Equal(1, match.Round);
        Assert.Equal(1, match.BracketPosition);
        Assert.Equal(participant1Id, match.Participant1Id);
        Assert.Equal(participant2Id, match.Participant2Id);
        Assert.Null(match.WinnerParticipationId);
        Assert.Null(match.Participant1Score);
        Assert.Null(match.Participant2Score);
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
                1,
                1,
                participant1Id,
                participant2Id));
    }

    [Fact]
    public void Constructor_With_Zero_Round_Should_Throw()
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
                0,
                1,
                participant1Id,
                participant2Id));
    }

    [Fact]
    public void Constructor_With_Negative_Round_Should_Throw()
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
                -1,
                1,
                participant1Id,
                participant2Id));
    }

    [Fact]
    public void Constructor_With_Zero_BracketPosition_Should_Throw()
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
                1,
                0,
                participant1Id,
                participant2Id));
    }

    [Fact]
    public void Constructor_With_Negative_BracketPosition_Should_Throw()
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
                1,
                -1,
                participant1Id,
                participant2Id));
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
                1,
                1,
                participantId,
                participantId));
    }

    [Fact]
    public void RecordResult_With_Valid_Result_Should_Record_Result()
    {
        // Arrange
        var match = CreateMatch();

        // Act
        match.RecordResult(
            match.Participant1Id,
            9,
            5);

        // Assert
        Assert.Equal(
            match.Participant1Id,
            match.WinnerParticipationId);
        Assert.Equal(9, match.Participant1Score);
        Assert.Equal(5, match.Participant2Score);
    }

    [Fact]
    public void RecordResult_With_Winner_Not_In_Match_Should_Throw()
    {
        // Arrange
        var match = CreateMatch();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            match.RecordResult(
                ParticipationId.New(),
                9,
                5));
    }

    [Fact]
    public void RecordResult_With_Negative_Participant1Score_Should_Throw()
    {
        // Arrange
        var match = CreateMatch();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            match.RecordResult(
                match.Participant1Id,
                -1,
                5));
    }

    [Fact]
    public void RecordResult_With_Negative_Participant2Score_Should_Throw()
    {
        // Arrange
        var match = CreateMatch();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            match.RecordResult(
                match.Participant1Id,
                5,
                -1));
    }

    [Fact]
    public void RecordResult_When_Winner_Does_Not_Have_Higher_Score_Should_Throw()
    {
        // Arrange
        var match = CreateMatch();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            match.RecordResult(
                match.Participant1Id,
                5,
                5));
    }

    [Fact]
    public void RecordResult_When_Winner_Has_Lower_Score_Should_Throw()
    {
        // Arrange
        var match = CreateMatch();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            match.RecordResult(
                match.Participant1Id,
                5,
                9));
    }

    [Fact]
    public void RecordResult_When_Result_Already_Exists_Should_Throw()
    {
        // Arrange
        var match = CreateMatch();

        match.RecordResult(
            match.Participant1Id,
            9,
            5);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            match.RecordResult(
                match.Participant1Id,
                9,
                5));
    }

    private static Match CreateMatch()
    {
        return new Match(
            MatchId.New(),
            TournamentId.New(),
            1,
            1,
            ParticipationId.New(),
            ParticipationId.New());
    }
}
