using FluentValidation;

namespace HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;

public sealed class CreateOrganizationValidator
    : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
