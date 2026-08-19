using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Enums;
using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Domain.Entities;

/// <summary>
/// Represents a player's participation in a tournament.
/// </summary>
public sealed class Participation : Entity<ParticipationId>
{
    private Participation(
        ParticipationId id,
        PlayerId playerId,
        TournamentId tournamentId,
        DateOnly registrationDate,
        int? seed = null)
        : base(id)
    {
        if (registrationDate == default)
        {
            throw new ArgumentException(
                "Registration date is required.",
                nameof(registrationDate));
        }

        ValidateSeed(seed);

        PlayerId = playerId;
        TournamentId = tournamentId;
        RegistrationDate = registrationDate;
        Seed = seed;
        Status = ParticipationStatus.Registered;
    }

    /// <summary>
    /// Creates a new participation.
    /// </summary>
    /// <param name="playerId">The player identifier.</param>
    /// <param name="tournamentId">The tournament identifier.</param>
    /// <param name="registrationDate">The registration date.</param>
    /// <param name="seed">The player's seed.</param>
    /// <returns>A new participation.</returns>
    public static Participation Create(
        PlayerId playerId,
        TournamentId tournamentId,
        DateOnly registrationDate,
        int? seed = null)
        => new(
            ParticipationId.New(),
            playerId,
            tournamentId,
            registrationDate,
            seed);

    /// <summary>
    /// Updates the participation.
    /// </summary>
    /// <param name="seed">The player's seed.</param>
    /// <param name="status">The new participation status.</param>
    public void Update(
        int? seed,
        ParticipationStatus status)
    {
        ValidateSeed(seed);
        EnsureStatusTransition(status);

        if (Seed == seed &&
            Status == status)
        {
            return;
        }

        Seed = seed;
        Status = status;
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

    /// <summary>
    /// Gets the player's seed.
    /// </summary>
    public int? Seed { get; private set; }

    /// <summary>
    /// Gets the participation status.
    /// </summary>
    public ParticipationStatus Status { get; private set; }

    /// <summary>
    /// Gets the player.
    /// </summary>
    public Player Player { get; private set; } = default!;

    /// <summary>
    /// Gets the tournament.
    /// </summary>
    public HellenicAmericanPoolHistory.Domain.Tournament.Tournament Tournament
    {
        get;
        private set;
    } = default!;

    private static void ValidateSeed(int? seed)
    {
        if (seed.HasValue && seed.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(seed),
                "Seed must be greater than zero.");
        }
    }

    private void EnsureStatusTransition(
        ParticipationStatus newStatus)
    {
        if (Status == newStatus)
        {
            return;
        }

        var isAllowed = Status switch
        {
            ParticipationStatus.Registered =>
                newStatus is
                    ParticipationStatus.CheckedIn
                    or ParticipationStatus.Withdrawn
                    or ParticipationStatus.Disqualified,

            ParticipationStatus.CheckedIn =>
                newStatus is
                    ParticipationStatus.Withdrawn
                    or ParticipationStatus.Eliminated
                    or ParticipationStatus.Disqualified
                    or ParticipationStatus.Completed,

            ParticipationStatus.Withdrawn => false,

            ParticipationStatus.Eliminated => false,

            ParticipationStatus.Disqualified => false,

            ParticipationStatus.Completed => false,

            _ => false
        };

        if (!isAllowed)
        {
            throw new InvalidOperationException(
                $"Participation status cannot change from '{Status}' to '{newStatus}'.");
        }
    }
}