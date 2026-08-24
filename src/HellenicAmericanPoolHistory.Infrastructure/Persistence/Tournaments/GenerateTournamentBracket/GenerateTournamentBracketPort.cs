using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Enums;
using Microsoft.EntityFrameworkCore;

using TournamentEntity =
    HellenicAmericanPoolHistory.Domain.Tournament.Tournament;

using TournamentBracketType =
    HellenicAmericanPoolHistory.Domain.Tournament.BracketType;

using TournamentStatus =
    HellenicAmericanPoolHistory.Domain.Tournament.TournamentStatus;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GenerateTournamentBracket;

/// <summary>
/// Generates the first round of a single-elimination tournament.
/// </summary>
public sealed class GenerateTournamentBracketPort
    : IGenerateTournamentBracketPort
{
    private readonly ApplicationDbContext _context;

    public GenerateTournamentBracketPort(
        ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task GenerateAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken)
    {
        var tournament = await _context.Tournaments
            .FirstOrDefaultAsync(
                tournament => tournament.Id == tournamentId,
                cancellationToken);

        if (tournament is null)
        {
            throw new NotFoundException("Tournament not found.");
        }

        if (tournament.TournamentStatus !=
            TournamentStatus.InProgress)
        {
            throw new ConflictException(
                "Tournament bracket can only be generated while the tournament is in progress.");
        }

        if (tournament.BracketType !=
            TournamentBracketType.SingleElimination)
        {
            throw new ConflictException(
                "Bracket generation currently supports single-elimination tournaments only.");
        }

        var existingMatchExists = await _context.Matches
            .AnyAsync(
                match => match.TournamentId == tournamentId,
                cancellationToken);

        if (existingMatchExists)
        {
            throw new ConflictException(
                "Tournament bracket has already been generated.");
        }

        var participants = await _context.Participations
            .Where(participation =>
                participation.TournamentId == tournamentId &&
                participation.Status ==
                ParticipationStatus.CheckedIn)
            .ToListAsync(cancellationToken);

        if (participants.Count < 2)
        {
            throw new ConflictException(
                "At least two checked-in participants are required to generate the bracket.");
        }

        if (!IsPowerOfTwo(participants.Count))
        {
            throw new ConflictException(
                "Single-elimination bracket requires a number of participants that is a power of two.");
        }

        if (participants.Any(
                participation => !participation.Seed.HasValue))
        {
            throw new ConflictException(
                "All checked-in participants must have a seed.");
        }

        var orderedParticipants = participants
            .OrderBy(participation => participation.Seed)
            .ToList();

        if (orderedParticipants
            .Select(participation => participation.Seed!.Value)
            .Distinct()
            .Count() != orderedParticipants.Count)
        {
            throw new ConflictException(
                "Participant seeds must be unique.");
        }

        var expectedSeeds = Enumerable
            .Range(1, orderedParticipants.Count)
            .ToHashSet();

        var actualSeeds = orderedParticipants
            .Select(participation => participation.Seed!.Value)
            .ToHashSet();

        if (!actualSeeds.SetEquals(expectedSeeds))
        {
            throw new ConflictException(
                "Participant seeds must be consecutive starting at 1.");
        }

        var matches = new List<Match>();

        for (var index = 0;
             index < orderedParticipants.Count / 2;
             index++)
        {
            var participant1 = orderedParticipants[index];

            var participant2 =
                orderedParticipants[
                    orderedParticipants.Count - 1 - index];

            matches.Add(
                new Match(
                    MatchId.New(),
                    tournamentId,
                    1,
                    index + 1,
                    participant1.Id,
                    participant2.Id));
        }

        _context.Matches.AddRange(matches);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static bool IsPowerOfTwo(int value)
        => value > 0 &&
           (value & (value - 1)) == 0;
}
