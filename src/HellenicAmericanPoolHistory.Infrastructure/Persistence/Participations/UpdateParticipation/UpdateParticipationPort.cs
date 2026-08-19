using HellenicAmericanPoolHistory.Application.Common.Exceptions;
using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using Microsoft.EntityFrameworkCore;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.UpdateParticipation;

/// <summary>
/// Updates participations in the database.
/// </summary>
public sealed class UpdateParticipationPort : IUpdateParticipationPort
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UpdateParticipationPort"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public UpdateParticipationPort(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    /// <inheritdoc />
    public async Task<UpdateParticipationResponse?> UpdateAsync(
        UpdateParticipationCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var participation = await _context.Participations
            .FirstOrDefaultAsync(
                participation => participation.Id == command.ParticipationId,
                cancellationToken);

        if (participation is null)
        {
            throw new NotFoundException("Participation not found.");
        }

        try
        {
            participation.Update(
                command.Seed,
                command.Status);
        }
        catch (InvalidOperationException exception)
        {
            throw new ConflictException(exception.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateParticipationResponse(
            participation.Id.Value);
    }
}