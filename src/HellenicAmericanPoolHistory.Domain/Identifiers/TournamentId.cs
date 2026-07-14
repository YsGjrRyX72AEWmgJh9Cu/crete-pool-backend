namespace HellenicAmericanPoolHistory.Domain.Identifiers;

/// <summary>
/// Strongly typed identifier for a tournament.
/// </summary>
public readonly record struct TournamentId(Guid Value)
{
    public static TournamentId New()
        => new(Guid.NewGuid());

    public override string ToString()
        => Value.ToString();
}