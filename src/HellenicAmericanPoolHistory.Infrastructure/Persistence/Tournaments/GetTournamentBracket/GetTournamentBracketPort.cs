using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournamentBracket;

/// <summary>
/// Retrieves tournament brackets from the database.
/// </summary>
public sealed class GetTournamentBracketPort : IGetTournamentBracketPort
{
    private readonly ApplicationDbContext _dbContext;

    public GetTournamentBracketPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<GetTournamentBracketResponse?> GetByTournamentIdAsync(
        TournamentId tournamentId,
        CancellationToken cancellationToken)
    {
        var tournament = await _dbContext.Tournaments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                tournament => tournament.Id == tournamentId,
                cancellationToken);

        if (tournament is null)
        {
            return null;
        }

        var matches = await _dbContext.Matches
            .AsNoTracking()
            .Where(match => match.TournamentId == tournamentId)
            .Include(match => match.Participant1)
                .ThenInclude(participation => participation.Player)
            .Include(match => match.Participant2)
                .ThenInclude(participation => participation.Player)
            .Include(match => match.Winner)
                .ThenInclude(participation => participation.Player)
            .OrderBy(match => match.Round)
            .ThenBy(match => match.BracketPosition)
            .ToListAsync(cancellationToken);

        var rounds = matches
            .GroupBy(match => match.Round)
            .OrderBy(group => group.Key)
            .Select(group =>
                new GetTournamentBracketRoundResponse(
                    group.Key,
                    group
                        .OrderBy(match => match.BracketPosition)
                        .Select(match =>
                            new GetTournamentBracketMatchResponse(
                                match.Id.Value,
                                match.BracketPosition,
                                match.Participant1Id.Value,
                                match.Participant1.Player.FullName,
                                match.Participant2Id.Value,
                                match.Participant2.Player.FullName,
                                match.WinnerParticipationId?.Value,
                                match.Winner?.Player.FullName,
                                match.Participant1Score,
                                match.Participant2Score))
                        .ToList()))
            .ToList();

        return new GetTournamentBracketResponse(
            tournament.Id.Value,
            tournament.Name,
            rounds);
    }
}
