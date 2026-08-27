using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;
using HellenicAmericanPoolHistory.Domain.Organization;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Organizations.GetOrganization;

public sealed class GetOrganizationHandlerTests
{
    [Fact]
    public async Task HandleAsync_With_Valid_Query_Should_Return_Organization()
    {
        var organizationId = OrganizationId.New();

        var expectedResponse = new GetOrganizationResponse(
            organizationId.Value,
            "Test Organization");

        var port = new FakeGetOrganizationPort(
            expectedResponse);

        var handler = new GetOrganizationHandler(port);

        var query = new GetOrganizationQuery(
            organizationId.Value);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.NotNull(response);

        Assert.Equal(
            expectedResponse,
            response);

        Assert.Equal(
            organizationId.Value,
            response.Id);
    }

    [Fact]
    public async Task HandleAsync_When_Organization_Does_Not_Exist_Should_Return_Null()
    {
        var port = new FakeGetOrganizationPort(null);

        var handler = new GetOrganizationHandler(port);

        var query = new GetOrganizationQuery(
            OrganizationId.New().Value);

        var response = await handler.HandleAsync(
            query,
            CancellationToken.None);

        Assert.Null(response);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_Organization_Id_To_Port()
    {
        var organizationId = OrganizationId.New();

        var port = new FakeGetOrganizationPort(null);

        var handler = new GetOrganizationHandler(port);

        await handler.HandleAsync(
            new GetOrganizationQuery(
                organizationId.Value),
            CancellationToken.None);

        Assert.Equal(
            organizationId.Value,
            port.RequestedOrganizationId);
    }

    private sealed class FakeGetOrganizationPort
        : IGetOrganizationPort
    {
        private readonly GetOrganizationResponse? _response;

        public FakeGetOrganizationPort(
            GetOrganizationResponse? response)
        {
            _response = response;
        }

        public Guid? RequestedOrganizationId { get; private set; }

        public Task<GetOrganizationResponse?> GetByIdAsync(
            Guid organizationId,
            CancellationToken cancellationToken)
        {
            RequestedOrganizationId = organizationId;

            return Task.FromResult(_response);
        }
    }
}
