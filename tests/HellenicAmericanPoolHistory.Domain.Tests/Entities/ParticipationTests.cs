using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Xunit;

namespace HellenicAmericanPoolHistory.Domain.Tests.Entities;

/// <summary>
/// Unit tests for the <see cref="Participation"/> entity.
/// </summary>
public class ParticipationTests
{
    [Fact]
    public void Constructor_Should_Create_Participation()
    {
        // Arrange
        var id = ParticipationId.New();
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();
        var registrationDate = new DateOnly(2026, 7, 16);

        // Act
        var participation = new Participation(
            id,
            playerId,
            tournamentId,
            registrationDate);

        // Assert
        Assert.Equal(id, participation.Id);
        Assert.Equal(playerId, participation.PlayerId);
        Assert.Equal(tournamentId, participation.TournamentId);
        Assert.Equal(registrationDate, participation.RegistrationDate);
    }

    [Fact]
    public void Constructor_Should_Throw_When_RegistrationDate_Is_Default()
    {
        // Arrange
        var id = ParticipationId.New();
        var playerId = PlayerId.New();
        var tournamentId = TournamentId.New();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Participation(
                id,
                playerId,
                tournamentId,
                default));
    }
}