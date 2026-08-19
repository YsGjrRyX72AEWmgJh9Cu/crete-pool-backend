namespace HellenicAmericanPoolHistory.Domain.Enums;

/// <summary>
/// Represents the status of a player's participation in a tournament.
/// </summary>
public enum ParticipationStatus
{
    Registered = 1,
    CheckedIn = 2,
    Withdrawn = 3,
    Eliminated = 4,
    Disqualified = 5,
    Completed = 6
}