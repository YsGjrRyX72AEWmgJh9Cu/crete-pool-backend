using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Identifiers;

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
    /// <param name="participant1Id">The first participant.</param>
    /// <param name="participant2Id">The second participant.</param>
    /// <param name="winnerParticipationId">The winning participant.</param>
    /// <param name="participant1Score">The first participant's score.</param>
    /// <param name="participant2Score">The second participant's score.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description>The same participant is assigned twice.</description></item>
    /// <item><description>The winner is not one of the participants.</description></item>
    /// <item><description>The winner does not have the higher score.</description></item>
    /// </list>
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when a score is negative.
    /// </exception>
    public Match(
        MatchId id,
        ParticipationId participant1Id,
        ParticipationId participant2Id,
        ParticipationId winnerParticipationId,
        int participant1Score,
        int participant2Score)
        : base(id)
    {
        // Validation

        if (participant1Id == participant2Id)
        {
            throw new ArgumentException(
                "A match cannot have the same participant twice.",
                nameof(participant2Id));
        }

        if (winnerParticipationId != participant1Id &&
            winnerParticipationId != participant2Id)
        {
            throw new ArgumentException(
                "Winner must be one of the match participants.",
                nameof(winnerParticipationId));
        }

        if (participant1Score < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(participant1Score));
        }

        if (participant2Score < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(participant2Score));
        }

        var winnerHasHigherScore =
            winnerParticipationId == participant1Id
                ? participant1Score > participant2Score
                : participant2Score > participant1Score;

        if (!winnerHasHigherScore)
        {
            throw new ArgumentException(
                "Winner must have the higher score.",
                nameof(winnerParticipationId));
        }

        // Assignments

        Participant1Id = participant1Id;
        Participant2Id = participant2Id;
        WinnerParticipationId = winnerParticipationId;
        Participant1Score = participant1Score;
        Participant2Score = participant2Score;
    }

    /// <summary>
    /// Gets the first participant.
    /// </summary>
    public ParticipationId Participant1Id { get; }

    /// <summary>
    /// Gets the second participant.
    /// </summary>
    public ParticipationId Participant2Id { get; }

    /// <summary>
    /// Gets the winning participant.
    /// </summary>
    public ParticipationId WinnerParticipationId { get; }

    /// <summary>
    /// Gets the first participant's score.
    /// </summary>
    public int Participant1Score { get; }

    /// <summary>
    /// Gets the second participant's score.
    /// </summary>
    public int Participant2Score { get; }
}