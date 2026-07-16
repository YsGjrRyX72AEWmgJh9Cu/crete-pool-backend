using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Entities;

/// <summary>
/// Represents a player's participation in a tournament.
/// </summary>
public sealed class Participation : Entity<ParticipationId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Participation"/> class.
    /// </summary>
    /// <param name="id">The participation identifier.</param>
    /// <param name="playerId">The player identifier.</param>
    /// <param name="tournamentId">The tournament identifier.</param>
    /// <param name="registrationDate">The registration date.</param>
    public Participation(
        ParticipationId id,
        PlayerId playerId,
        TournamentId tournamentId,
        DateOnly registrationDate)
        : base(id)
    {
        if (registrationDate == default)
        {
            throw new ArgumentException(
                "Registration date is required.",
                nameof(registrationDate));
        }

        PlayerId = playerId;
        TournamentId = tournamentId;
        RegistrationDate = registrationDate;
    }

    /// <summary>
    /// Gets the player identifier.
    /// </summary>
    public PlayerId PlayerId { get; }

    /// <summary>
    /// Gets the tournament identifier.
    /// </summary>
    public TournamentId TournamentId { get; }

    /// <summary>
    /// Gets the registration date.
    /// </summary>
    public DateOnly RegistrationDate { get; }
}