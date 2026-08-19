using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;
using Xunit;

using TournamentEntity =
    HellenicAmericanPoolHistory.Domain.Tournament.Tournament;

namespace HellenicAmericanPoolHistory.Domain.Tests.Tournaments;

/// <summary>
/// Unit tests for the Tournament entity.
/// </summary>
public sealed class TournamentTests
{
    [Fact]
    public void Create_Should_Create_Tournament_In_Draft_Status()
    {
        // Arrange
        var data = CreateTournamentData();

        // Act
        var tournament = TournamentEntity.Create(data);

        // Assert
        Assert.NotEqual(default, tournament.Id);
        Assert.Equal(data.Name, tournament.Name);
        Assert.Equal(data.TournamentType, tournament.TournamentType);
        Assert.Equal(data.BracketType, tournament.BracketType);
        Assert.Equal(data.GameSet, tournament.GameSet);
        Assert.Equal(
            TournamentStatus.Draft,
            tournament.TournamentStatus);
        Assert.Equal(data.StartDate, tournament.StartDate);
        Assert.Equal(data.EndDate, tournament.EndDate);
        Assert.Equal(data.VenueId, tournament.VenueId);
    }

    [Fact]
    public void Create_Should_Throw_When_EndDate_Is_Before_StartDate()
    {
        // Arrange
        var data = CreateTournamentData(
            startDate: new DateOnly(2026, 8, 20),
            endDate: new DateOnly(2026, 8, 19));

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            TournamentEntity.Create(data));
    }

    [Fact]
    public void Rename_Should_Trim_Name()
    {
        // Arrange
        var tournament = CreateTournament();

        // Act
        tournament.Rename("  Updated Tournament  ");

        // Assert
        Assert.Equal(
            "Updated Tournament",
            tournament.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        // Arrange
        var tournament = CreateTournament();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            tournament.Rename("   "));
    }

    [Fact]
    public void Schedule_Should_Move_Draft_To_Scheduled()
    {
        // Arrange
        var tournament = CreateTournament();

        // Act
        tournament.Schedule();

        // Assert
        Assert.Equal(
            TournamentStatus.Scheduled,
            tournament.TournamentStatus);
    }

    [Fact]
    public void Start_Should_Move_Scheduled_To_InProgress()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();

        // Act
        tournament.Start();

        // Assert
        Assert.Equal(
            TournamentStatus.InProgress,
            tournament.TournamentStatus);
    }

    [Fact]
    public void Complete_Should_Move_InProgress_To_Completed()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();
        tournament.Start();

        // Act
        tournament.Complete();

        // Assert
        Assert.Equal(
            TournamentStatus.Completed,
            tournament.TournamentStatus);
    }

    [Fact]
    public void Cancel_Should_Move_Draft_To_Cancelled()
    {
        // Arrange
        var tournament = CreateTournament();

        // Act
        tournament.Cancel();

        // Assert
        Assert.Equal(
            TournamentStatus.Cancelled,
            tournament.TournamentStatus);
    }

    [Fact]
    public void Cancel_Should_Move_Scheduled_To_Cancelled()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();

        // Act
        tournament.Cancel();

        // Assert
        Assert.Equal(
            TournamentStatus.Cancelled,
            tournament.TournamentStatus);
    }

    [Fact]
    public void Schedule_Should_Reject_NonDraft_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Schedule());
    }

    [Fact]
    public void Start_Should_Reject_Draft_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Start());
    }

    [Fact]
    public void Start_Should_Reject_InProgress_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();
        tournament.Start();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Start());
    }

    [Fact]
    public void Complete_Should_Reject_Scheduled_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Complete());
    }

    [Fact]
    public void Complete_Should_Reject_Completed_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();
        tournament.Start();
        tournament.Complete();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Complete());
    }

    [Fact]
    public void Cancel_Should_Reject_InProgress_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();
        tournament.Start();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Cancel());
    }

    [Fact]
    public void Cancel_Should_Reject_Completed_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();
        tournament.Start();
        tournament.Complete();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Cancel());
    }

    [Fact]
    public void Cancel_Should_Reject_Cancelled_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Cancel());
    }

    [Fact]
    public void Edit_Should_Update_Tournament_While_In_Draft()
    {
        // Arrange
        var tournament = CreateTournament();

        var newVenueId = VenueId.New();

        var data = CreateTournamentData(
            name: "Updated Tournament",
            startDate: new DateOnly(2026, 9, 1),
            endDate: new DateOnly(2026, 9, 2),
            venueId: newVenueId);

        // Act
        tournament.Edit(data);

        // Assert
        Assert.Equal(
            "Updated Tournament",
            tournament.Name);

        Assert.Equal(
            data.TournamentType,
            tournament.TournamentType);

        Assert.Equal(
            data.BracketType,
            tournament.BracketType);

        Assert.Equal(
            data.GameSet,
            tournament.GameSet);

        Assert.Equal(
            data.StartDate,
            tournament.StartDate);

        Assert.Equal(
            data.EndDate,
            tournament.EndDate);

        Assert.Equal(
            data.VenueId,
            tournament.VenueId);
    }

    [Fact]
    public void Edit_Should_Reject_Scheduled_Tournament()
    {
        // Arrange
        var tournament = CreateTournament();

        tournament.Schedule();

        var data = CreateTournamentData(
            name: "Updated Tournament");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tournament.Edit(data));
    }

    [Fact]
    public void Edit_Should_Reject_Invalid_Date_Range()
    {
        // Arrange
        var tournament = CreateTournament();

        var data = CreateTournamentData(
            startDate: new DateOnly(2026, 9, 2),
            endDate: new DateOnly(2026, 9, 1));

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            tournament.Edit(data));
    }

    private static TournamentEntity CreateTournament()
        => TournamentEntity.Create(
            CreateTournamentData());

    private static TournamentData CreateTournamentData(
        string name = "Test Tournament",
        TournamentType tournamentType = TournamentType.Individual,
        BracketType bracketType = BracketType.SingleElimination,
        GameSet gameSet = GameSet.RaceTo5,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        VenueId? venueId = null)
        => new(
            name,
            tournamentType,
            bracketType,
            gameSet,
            startDate ?? new DateOnly(2026, 8, 11),
            endDate ?? new DateOnly(2026, 8, 11),
            venueId ?? VenueId.New());
}