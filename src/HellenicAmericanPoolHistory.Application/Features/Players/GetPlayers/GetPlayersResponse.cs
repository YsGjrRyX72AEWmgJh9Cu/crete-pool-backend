namespace HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;

public sealed record GetPlayersResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Country,
    DateOnly? BirthDate);