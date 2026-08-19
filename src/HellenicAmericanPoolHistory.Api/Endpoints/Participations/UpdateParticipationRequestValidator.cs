using FluentValidation;

namespace HellenicAmericanPoolHistory.Api.Endpoints.Participations;

public sealed class UpdateParticipationRequestValidator
    : AbstractValidator<UpdateParticipationRequest>
{
    public UpdateParticipationRequestValidator()
    {
        RuleFor(request => request.Seed)
            .GreaterThan(0)
            .When(request => request.Seed.HasValue)
            .WithMessage("Seed must be greater than zero.");

        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage("Participation status is invalid.");
    }
}