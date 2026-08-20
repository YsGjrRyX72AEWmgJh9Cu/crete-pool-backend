namespace HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;

/// <summary>
/// Represents the request to create a new match.
/// </summary>
/// <param name="TournamentId">The tournament identifier.</param>
/// <param name="Participant1Id">The first participation identifier.</param>
/// <param name="Participant2Id">The second participation identifier.</param>
/// <param name="WinnerParticipationId">
/// The winning participation identifier, when a result exists.
/// </param>
/// <param name="Participant1Score">
/// The first participant's score, when a result exists.
/// </param>
/// <param name="Participant2Score">
/// The second participant's score, when a result exists.
/// </param>
public sealed record CreateMatchCommand(
    Guid TournamentId,
    Guid Participant1Id,
    Guid Participant2Id,
    Guid? WinnerParticipationId,
    int? Participant1Score,
    int? Participant2Score);
