using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Players.CreatePlayer;

public sealed class CreatePlayerCommandValidatorTests
{
    private readonly CreatePlayerCommandValidator _validator = new();

    [Fact]
    public async Task Validate_Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            "Greece",
            new DateOnly(1990, 1, 1));

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_Should_Pass_When_BirthDate_Is_Null()
    {
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            "Greece",
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_Should_Fail_When_FirstName_Is_Empty()
    {
        var command = new CreatePlayerCommand(
            string.Empty,
            "Doe",
            "Greece",
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.FirstName));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_FirstName_Exceeds_Maximum_Length()
    {
        var command = new CreatePlayerCommand(
            new string('A', 101),
            "Doe",
            "Greece",
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.FirstName));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_LastName_Is_Empty()
    {
        var command = new CreatePlayerCommand(
            "John",
            string.Empty,
            "Greece",
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.LastName));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_LastName_Exceeds_Maximum_Length()
    {
        var command = new CreatePlayerCommand(
            "John",
            new string('B', 101),
            "Greece",
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.LastName));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_CountryOfOrigin_Is_Empty()
    {
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            string.Empty,
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.CountryOfOrigin));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_CountryOfOrigin_Exceeds_Maximum_Length()
    {
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            new string('C', 101),
            null);

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName ==
                nameof(command.CountryOfOrigin));
    }

    [Fact]
    public async Task Validate_Should_Fail_When_BirthDate_Is_In_The_Future()
    {
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            "Greece",
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)));

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.BirthDate));
    }

    [Fact]
    public async Task Validate_Should_Pass_When_BirthDate_Is_Today()
    {
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            "Greece",
            DateOnly.FromDateTime(DateTime.Today));

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }
}
