using HellenicAmericanPoolHistory.Domain.Common.Abstractions;

namespace HellenicAmericanPoolHistory.Domain.Tournament;

public readonly record struct TournamentId(Guid Value)
    : IStronglyTypedId<Guid>
{
    public static TournamentId New()
        => new(Guid.NewGuid());

    public override string ToString()
        => Value.ToString();
}