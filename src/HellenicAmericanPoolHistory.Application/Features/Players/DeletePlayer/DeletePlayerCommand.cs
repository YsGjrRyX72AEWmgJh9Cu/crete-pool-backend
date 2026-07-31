using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;

public sealed record DeletePlayerCommand(
    PlayerId Id);