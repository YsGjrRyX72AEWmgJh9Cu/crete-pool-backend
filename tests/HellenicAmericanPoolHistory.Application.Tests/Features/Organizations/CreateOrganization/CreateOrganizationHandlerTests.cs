using HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;
using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Organizations.CreateOrganization;

public sealed class CreateOrganizationHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Create_Organization_And_Return_Id()
    {
        var port = new FakeCreateOrganizationPort();

        var handler = new CreateOrganizationHandler(port);

        var command = new CreateOrganizationCommand(
            "Test Organization");

        var response = await handler.HandleAsync(
            command,
            CancellationToken.None);

        Assert.NotEqual(
            Guid.Empty,
            response.OrganizationId);

        Assert.NotNull(
            port.SavedOrganization);

        Assert.Equal(
            command.Name,
            port.SavedOrganization.Name);

        Assert.Equal(
            response.OrganizationId,
            port.SavedOrganization.Id.Value);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationToken =
            new CancellationTokenSource().Token;

        var port = new FakeCreateOrganizationPort();

        var handler = new CreateOrganizationHandler(port);

        await handler.HandleAsync(
            new CreateOrganizationCommand("Test Organization"),
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.CancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Command_Is_Null()
    {
        var port = new FakeCreateOrganizationPort();

        var handler = new CreateOrganizationHandler(port);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(
                null!,
                CancellationToken.None));
    }

    private sealed class FakeCreateOrganizationPort
        : ICreateOrganizationPort
    {
        public OrganizationEntity? SavedOrganization { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task SaveAsync(
            OrganizationEntity organization,
            CancellationToken cancellationToken)
        {
            SavedOrganization = organization;
            CancellationToken = cancellationToken;

            return Task.CompletedTask;
        }
    }
}
