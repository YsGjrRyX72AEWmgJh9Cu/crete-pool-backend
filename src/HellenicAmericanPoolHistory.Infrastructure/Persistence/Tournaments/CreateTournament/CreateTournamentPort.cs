using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Domain.Tournament;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CreateTournament;

/// <summary>
/// Persists newly created tournaments.
/// </summary>
public sealed class CreateTournamentPort : ICreateTournamentPort
{
    private readonly ApplicationDbContext _dbContext;

    public CreateTournamentPort(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task SaveAsync(
        Tournament tournament,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tournament);

        await _dbContext.Tournaments.AddAsync(
            tournament,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}