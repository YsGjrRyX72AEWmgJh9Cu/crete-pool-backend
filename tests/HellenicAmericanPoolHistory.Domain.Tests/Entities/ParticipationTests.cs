using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Xunit;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

/// <summary>
/// Unit tests for the <see cref="Participation"/> entity.
/// </summary>
public class ParticipationTests
{
    [Fact]
    public void Create_Should_Create_Participation()
    {
        // Arrange
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();
        var registrationDate = new DateOnly(2026, 7, 16);

        // Act
        var participation = Participation.Create(
            playerId,
            tournamentId,
            registrationDate);

        // Assert
        Assert.NotEqual(default, participation.Id);
        Assert.Equal(playerId, participation.PlayerId);
        Assert.Equal(tournamentId, participation.TournamentId);
        Assert.Equal(registrationDate, participation.RegistrationDate);
        Assert.Null(participation.Seed);
        Assert.Equal(
            ParticipationStatus.Registered,
            participation.Status);
    }

    [Fact]
    public void Create_Should_Throw_When_RegistrationDate_Is_Default()
    {
        // Arrange
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Participation.Create(
                playerId,
                tournamentId,
                default));
    }

    [Fact]
    public void Create_Should_Throw_When_Seed_Is_Zero()
    {
        // Arrange
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();
        var registrationDate = new DateOnly(2026, 7, 16);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Participation.Create(
                playerId,
                tournamentId,
                registrationDate,
                0));
    }

    [Fact]
    public void Create_Should_Throw_When_Seed_Is_Negative()
    {
        // Arrange
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();
        var registrationDate = new DateOnly(2026, 7, 16);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Participation.Create(
                playerId,
                tournamentId,
                registrationDate,
                -1));
    }

    [Fact]
    public void Update_Should_Allow_Registered_To_CheckedIn()
    {
        // Arrange
        var participation = CreateParticipation();

        // Act
        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        // Assert
        Assert.Equal(1, participation.Seed);
        Assert.Equal(
            ParticipationStatus.CheckedIn,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Allow_Registered_To_Withdrawn()
    {
        // Arrange
        var participation = CreateParticipation();

        // Act
        participation.Update(
            null,
            ParticipationStatus.Withdrawn);

        // Assert
        Assert.Null(participation.Seed);
        Assert.Equal(
            ParticipationStatus.Withdrawn,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Allow_Registered_To_Disqualified()
    {
        // Arrange
        var participation = CreateParticipation();

        // Act
        participation.Update(
            null,
            ParticipationStatus.Disqualified);

        // Assert
        Assert.Equal(
            ParticipationStatus.Disqualified,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Allow_CheckedIn_To_Eliminated()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        // Act
        participation.Update(
            1,
            ParticipationStatus.Eliminated);

        // Assert
        Assert.Equal(
            ParticipationStatus.Eliminated,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Allow_CheckedIn_To_Disqualified()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        // Act
        participation.Update(
            null,
            ParticipationStatus.Disqualified);

        // Assert
        Assert.Equal(
            ParticipationStatus.Disqualified,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Allow_CheckedIn_To_Completed()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        // Act
        participation.Update(
            1,
            ParticipationStatus.Completed);

        // Assert
        Assert.Equal(
            ParticipationStatus.Completed,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Allow_CheckedIn_To_Withdrawn()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        // Act
        participation.Update(
            null,
            ParticipationStatus.Withdrawn);

        // Assert
        Assert.Equal(
            ParticipationStatus.Withdrawn,
            participation.Status);
    }

    [Fact]
    public void Update_Should_Reject_Transition_From_Withdrawn()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            null,
            ParticipationStatus.Withdrawn);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                ParticipationStatus.CheckedIn));
    }

    [Fact]
    public void Update_Should_Reject_Transition_From_Eliminated()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        participation.Update(
            1,
            ParticipationStatus.Eliminated);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                ParticipationStatus.CheckedIn));
    }

    [Fact]
    public void Update_Should_Reject_Transition_From_Disqualified()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            null,
            ParticipationStatus.Disqualified);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                ParticipationStatus.CheckedIn));
    }

    [Fact]
    public void Update_Should_Reject_Transition_From_Completed()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        participation.Update(
            1,
            ParticipationStatus.Completed);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                ParticipationStatus.CheckedIn));
    }

    [Fact]
    public void Update_Should_Reject_Registered_To_Eliminated()
    {
        // Arrange
        var participation = CreateParticipation();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                ParticipationStatus.Eliminated));
    }

    [Fact]
    public void Update_Should_Reject_Registered_To_Completed()
    {
        // Arrange
        var participation = CreateParticipation();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                ParticipationStatus.Completed));
    }

    [Fact]
    public void Update_Should_Reject_Registered_To_Unknown_Status()
    {
        // Arrange
        var participation = CreateParticipation();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            participation.Update(
                1,
                (ParticipationStatus)999));
    }

    [Fact]
    public void Update_With_Same_Seed_And_Status_Should_Be_NoOp()
    {
        // Arrange
        var participation = CreateParticipation();

        participation.Update(
            1,
            ParticipationStatus.CheckedIn);

        var originalSeed = participation.Seed;
        var originalStatus = participation.Status;

        // Act
        participation.Update(
            originalSeed,
            originalStatus);

        // Assert
        Assert.Equal(originalSeed, participation.Seed);
        Assert.Equal(originalStatus, participation.Status);
    }

    private static Participation CreateParticipation()
    {
        return Participation.Create(
            PlayerId.New(),
            TournamentId.New(),
            new DateOnly(2026, 7, 16));
    }
}
