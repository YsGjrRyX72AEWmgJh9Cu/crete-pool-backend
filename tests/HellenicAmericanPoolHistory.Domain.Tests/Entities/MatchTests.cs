using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

/// <summary>
/// Unit tests for the <see cref="Match"/>.
/// </summary>
public class MatchTests
{
    [Fact]
    public void Constructor_With_Valid_Values_Should_Create_Match()
    {
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        var match = new Match(
            MatchId.New(),
            participant1Id,
            participant2Id,
            participant1Id,
            9,
            5);

        Assert.Equal(participant1Id, match.Participant1Id);
        Assert.Equal(participant2Id, match.Participant2Id);
        Assert.Equal(participant1Id, match.WinnerParticipationId);
        Assert.Equal(9, match.Participant1Score);
        Assert.Equal(5, match.Participant2Score);
    }

    [Fact]
    public void Constructor_With_Same_Participants_Should_Throw()
    {
        var participantId = ParticipationId.New();

        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                participantId,
                participantId,
                participantId,
                9,
                0));
    }

    [Fact]
    public void Constructor_With_Winner_Not_In_Match_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                ParticipationId.New(),
                ParticipationId.New(),
                ParticipationId.New(),
                9,
                5));
    }

    [Fact]
    public void Constructor_With_Negative_Participant1Score_Should_Throw()
    {
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Match(
                MatchId.New(),
                participant1Id,
                participant2Id,
                participant1Id,
                -1,
                5));
    }

    [Fact]
    public void Constructor_With_Negative_Participant2Score_Should_Throw()
    {
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Match(
                MatchId.New(),
                participant1Id,
                participant2Id,
                participant1Id,
                5,
                -1));
    }

    [Fact]
    public void Constructor_When_Winner_Does_Not_Have_Higher_Score_Should_Throw()
    {
        var participant1Id = ParticipationId.New();
        var participant2Id = ParticipationId.New();

        Assert.Throws<ArgumentException>(() =>
            new Match(
                MatchId.New(),
                participant1Id,
                participant2Id,
                participant1Id,
                5,
                9));
    }
}