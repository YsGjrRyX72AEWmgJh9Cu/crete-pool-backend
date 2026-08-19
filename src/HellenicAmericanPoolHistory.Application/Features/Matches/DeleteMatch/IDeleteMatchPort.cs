using HellenicAmericanPoolHistory.Domain.Identifiers;

namespace HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;

public interface IDeleteMatchPort
{
    Task DeleteAsync(
        DeleteMatchCommand command,
        CancellationToken cancellationToken);
}
