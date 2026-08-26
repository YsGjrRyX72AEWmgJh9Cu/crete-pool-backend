using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;

/// <summary>
/// Handles the creation of a new organization.
/// </summary>
public sealed class CreateOrganizationHandler
{
    private readonly ICreateOrganizationPort _port;

    public CreateOrganizationHandler(
        ICreateOrganizationPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        _port = port;
    }

    public async Task<CreateOrganizationResponse> HandleAsync(
        CreateOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var organization = OrganizationEntity.Create(
            command.Name);

        await _port.SaveAsync(
            organization,
            cancellationToken);

        return new CreateOrganizationResponse(
            organization.Id.Value);
    }
}
