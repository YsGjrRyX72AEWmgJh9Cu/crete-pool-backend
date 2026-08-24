using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using TournamentEntity = HellenicAmericanPoolHistory.Domain.Tournament.Tournament;

namespace HellenicAmericanPoolHistory.Domain.Entities;

/// <summary>
/// Represents a match between two tournament participants.
/// </summary>
public sealed class Match : Entity<MatchId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Match"/> class.
    /// </summary>
    /// <param name="id">The unique match identifier.</param>
    /// <param name="tournamentId">The tournament identifier.</param>
    /// <param name="round">The tournament round number.</param>
    /// <param name="bracketPosition">
    /// The match position within the tournament round.
    /// </param>
    /// <param name="participant1Id">The first participant.</param>
    /// <param name="participant2Id">The second participant.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the tournament is missing or the same participant is
    /// assigned twice.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the round or bracket position is less than one.
    /// </exception>
    public Match(
        MatchId id,
        TournamentId tournamentId,
        int round,
        int bracketPosition,
        ParticipationId participant1Id,
        ParticipationId participant2Id)
        : base(id)
    {
        if (tournamentId == default)
        {
            throw new ArgumentException(
                "Tournament is required.",
                nameof(tournamentId));
        }

        if (round <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(round),
                "Round must be greater than zero.");
        }

        if (bracketPosition <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bracketPosition),
                "Bracket position must be greater than zero.");
        }

        if (participant1Id == participant2Id)
        {
            throw new ArgumentException(
                "A match cannot have the same participant twice.",
                nameof(participant2Id));
        }

        TournamentId = tournamentId;
        Round = round;
        BracketPosition = bracketPosition;
        Participant1Id = participant1Id;
        Participant2Id = participant2Id;
    }

    /// <summary>
    /// Records the result of the match.
    /// </summary>
    /// <param name="winnerParticipationId">
    /// The winning participant.
    /// </param>
    /// <param name="participant1Score">
    /// The first participant's score.
    /// </param>
    /// <param name="participant2Score">
    /// The second participant's score.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a result has already been recorded.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the winner is not one of the match participants or
    /// does not have the higher score.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when a score is negative.
    /// </exception>
    public void RecordResult(
        ParticipationId winnerParticipationId,
        int participant1Score,
        int participant2Score)
    {
        if (WinnerParticipationId.HasValue)
        {
            throw new InvalidOperationException(
                "Match result has already been recorded.");
        }

        if (winnerParticipationId != Participant1Id &&
            winnerParticipationId != Participant2Id)
        {
            throw new ArgumentException(
                "Winner must be one of the match participants.",
                nameof(winnerParticipationId));
        }

        if (participant1Score < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(participant1Score));
        }

        if (participant2Score < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(participant2Score));
        }

        var winnerHasHigherScore =
            winnerParticipationId == Participant1Id
                ? participant1Score > participant2Score
                : participant2Score > participant1Score;

        if (!winnerHasHigherScore)
        {
            throw new ArgumentException(
                "Winner must have the higher score.",
                nameof(winnerParticipationId));
        }

        WinnerParticipationId = winnerParticipationId;
        Participant1Score = participant1Score;
        Participant2Score = participant2Score;
    }

    /// <summary>
    /// Gets the tournament identifier.
    /// </summary>
    public TournamentId TournamentId { get; }

    /// <summary>
    /// Gets the tournament round number.
    /// </summary>
    public int Round { get; }

    /// <summary>
    /// Gets the match position within the tournament round.
    /// </summary>
    public int BracketPosition { get; }

    /// <summary>
    /// Gets the first participant identifier.
    /// </summary>
    public ParticipationId Participant1Id { get; }

    /// <summary>
    /// Gets the second participant identifier.
    /// </summary>
    public ParticipationId Participant2Id { get; }

    /// <summary>
    /// Gets the winning participant identifier, when a result has
    /// been recorded.
    /// </summary>
    public ParticipationId? WinnerParticipationId { get; private set; }

    /// <summary>
    /// Gets the first participant's score, when a result has been
    /// recorded.
    /// </summary>
    public int? Participant1Score { get; private set; }

    /// <summary>
    /// Gets the second participant's score, when a result has been
    /// recorded.
    /// </summary>
    public int? Participant2Score { get; private set; }

    /// <summary>
    /// Gets the tournament.
    /// </summary>
    public TournamentEntity Tournament { get; private set; } = default!;

    /// <summary>
    /// Gets the first participant.
    /// </summary>
    public Participation Participant1 { get; private set; } = default!;

    /// <summary>
    /// Gets the second participant.
    /// </summary>
    public Participation Participant2 { get; private set; } = default!;

    /// <summary>
    /// Gets the winning participant.
    /// </summary>
    public Participation Winner { get; private set; } = default!;
}
