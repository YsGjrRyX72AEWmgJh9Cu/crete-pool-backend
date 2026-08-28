using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Tournaments.CreateTournament;

public sealed class CreateTournamentValidatorTests
{
    private readonly CreateTournamentValidator _validator = new();

    [Fact]
    public async Task Validate_Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Name_Is_Empty()
    {
        var command = new CreateTournamentCommand(
            string.Empty,
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.Name));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Name_Exceeds_Maximum_Length()
    {
        var command = new CreateTournamentCommand(
            new string('A', 201),
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.Name));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_TournamentType_Is_Invalid()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            (TournamentType)999,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.TournamentType));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_BracketType_Is_Invalid()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            (BracketType)999,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.BracketType));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_GameSet_Is_Invalid()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            BracketType.SingleElimination,
            (GameSet)999,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.GameSet));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_StartDate_Is_Default()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            default,
            new DateOnly(2026, 9, 3),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.StartDate));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_EndDate_Is_Before_StartDate()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 3),
            new DateOnly(2026, 9, 1),
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.EndDate));
    }

    [Fact]
    public async Task Validate_Should_Pass_When_EndDate_Equals_StartDate()
    {
        var date = new DateOnly(2026, 9, 3);

        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            date,
            date,
            Guid.NewGuid(),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_Should_Fail_When_VenueId_Is_Empty()
    {
        var command = new CreateTournamentCommand(
            "Crete Open",
            TournamentType.Individual,
            BracketType.SingleElimination,
            GameSet.RaceTo7,
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 9, 3),
            Guid.Empty,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.VenueId));
    }
}
