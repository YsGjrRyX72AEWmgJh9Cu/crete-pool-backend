using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Matches.CreateMatch;

public sealed class CreateMatchValidatorTests
{
    private readonly CreateMatchValidator _validator = new();

    [Fact]
    public async Task Validate_Should_Pass_When_Required_Fields_Are_Valid_And_Result_Is_Omitted()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_Should_Pass_When_Winner_And_Both_Scores_Are_Provided()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            5,
            3);

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_Should_Fail_When_TournamentId_Is_Empty()
    {
        var command = new CreateMatchCommand(
            Guid.Empty,
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.TournamentId));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Round_Is_Not_Greater_Than_Zero()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            0,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.Round));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_BracketPosition_Is_Not_Greater_Than_Zero()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            0,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.BracketPosition));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Participant1Id_Is_Empty()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.Empty,
            Guid.NewGuid(),
            null,
            null,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.Participant1Id));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Participant2Id_Is_Empty()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.Empty,
            null,
            null,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.Participant2Id));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Participant1Score_Is_Negative()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            -1,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.Participant1Score));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Participant2Score_Is_Negative()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null,
            -1);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.Participant2Score));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Winner_Is_Provided_Without_Both_Scores()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            5,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.ErrorMessage ==
                "Winner and scores must either all be provided or all be omitted.");
    }

    [Fact]
    public async Task Validate_Should_Fail_When_Score_Is_Provided_Without_Winner()
    {
        var command = new CreateMatchCommand(
            Guid.NewGuid(),
            1,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            5,
            3);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.ErrorMessage ==
                "Winner and scores must either all be provided or all be omitted.");
    }
}
