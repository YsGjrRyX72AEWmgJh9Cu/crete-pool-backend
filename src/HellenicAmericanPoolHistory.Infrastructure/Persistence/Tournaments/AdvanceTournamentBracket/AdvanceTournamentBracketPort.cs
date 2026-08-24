using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Enums;
using Microsoft.EntityFrameworkCore;

using TournamentBracketType =
    HellenicAmericanPoolHistory.Domain.Tournament.BracketType;

using TournamentStatus =
    HellenicAmericanPoolHistory.Domain.Tournament.TournamentStatus;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.AdvanceTournamentBracket;

/// <summary>
/// Advances a single-elimination tournament bracket to the next round.
/// </summary>
public sealed class AdvanceTournamentBracketPort
    : IAdvanceTournamentBracketPort
{
    private readonly ApplicationDbContext _context;

    public AdvanceTournamentBracketPort(
        ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task AdvanceAsync(
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
                "Tournament bracket can only be advanced while the tournament is in progress.");
        }

        if (tournament.BracketType !=
            TournamentBracketType.SingleElimination)
        {
            throw new ConflictException(
                "Bracket advancement currently supports single-elimination tournaments only.");
        }

        var matches = await _context.Matches
            .Where(match =>
                match.TournamentId == tournamentId)
            .OrderBy(match => match.Round)
            .ThenBy(match => match.BracketPosition)
            .ToListAsync(cancellationToken);

        if (matches.Count == 0)
        {
            throw new ConflictException(
                "Tournament bracket has not been generated.");
        }

        var currentRound = matches
            .Max(match => match.Round);

        var currentRoundMatches = matches
            .Where(match => match.Round == currentRound)
            .OrderBy(match => match.BracketPosition)
            .ToList();

        if (currentRoundMatches.Any(
                match => !match.WinnerParticipationId.HasValue))
        {
            throw new ConflictException(
                "All matches in the current round must be completed before advancing.");
        }

        if (currentRoundMatches.Count == 1)
        {
            var final = currentRoundMatches[0];

            var winner = await _context.Participations
                .FirstOrDefaultAsync(
                    participation =>
                        participation.Id ==
                        final.WinnerParticipationId,
                    cancellationToken);

            if (winner is null)
            {
                throw new ConflictException(
                    "Winner participation could not be found.");
            }

            var loserId =
                winner.Id == final.Participant1Id
                    ? final.Participant2Id
                    : final.Participant1Id;

            var loser = await _context.Participations
                .FirstOrDefaultAsync(
                    participation =>
                        participation.Id == loserId,
                    cancellationToken);

           if (loser is null)
           {
                throw new ConflictException(
                    "Loser participation could not be found.");
           }

           try
           {
                winner.Update(
                    winner.Seed,
                    ParticipationStatus.Completed);

                 loser.Update(
                     loser.Seed,
                     ParticipationStatus.Eliminated);

                tournament.Complete();
           }
           catch (InvalidOperationException exception)
           {
           throw new ConflictException(exception.Message);
           }

           await _context.SaveChangesAsync(cancellationToken);

           return;
        }

        var nextRound = currentRound + 1;

        var nextRoundAlreadyExists = matches.Any(
            match => match.Round == nextRound);

        if (nextRoundAlreadyExists)
        {
            return;
        }

        var winners = currentRoundMatches
            .Select(match => match.WinnerParticipationId!.Value)
            .ToList();

        var nextRoundMatches = new List<Match>();

        for (var index = 0;
             index < winners.Count / 2;
             index++)
        {
            nextRoundMatches.Add(
                new Match(
                    MatchId.New(),
                    tournamentId,
                    nextRound,
                    index + 1,
                    winners[index * 2],
                    winners[index * 2 + 1]));
        }

        _context.Matches.AddRange(nextRoundMatches);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
