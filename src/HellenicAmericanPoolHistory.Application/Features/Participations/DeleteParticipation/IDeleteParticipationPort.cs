namespace HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;

public interface IDeleteParticipationPort
{
    Task DeleteAsync(
        DeleteParticipationCommand command,
        CancellationToken cancellationToken);
}