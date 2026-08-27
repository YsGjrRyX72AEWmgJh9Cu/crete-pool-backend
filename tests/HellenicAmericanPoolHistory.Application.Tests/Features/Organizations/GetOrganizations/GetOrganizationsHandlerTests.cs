using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.Organizations.GetOrganizations;

public sealed class GetOrganizationsHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Organizations_From_Port()
    {
        var organizations =
            new List<GetOrganizationsResponse>
            {
                new(
                    Guid.NewGuid(),
                    "Organization A"),

                new(
                    Guid.NewGuid(),
                    "Organization B")
            };

        var port = new FakeGetOrganizationsPort(organizations);

        var handler = new GetOrganizationsHandler(port);

        var result = await handler.HandleAsync(
            CancellationToken.None);

        Assert.Equal(
            organizations,
            result);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Empty_List_When_Port_Returns_Empty_List()
    {
        var organizations =
            Array.Empty<GetOrganizationsResponse>();

        var port = new FakeGetOrganizationsPort(organizations);

        var handler = new GetOrganizationsHandler(port);

        var result = await handler.HandleAsync(
            CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Port()
    {
        var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var port =
            new FakeGetOrganizationsPort(
                Array.Empty<GetOrganizationsResponse>());

        var handler = new GetOrganizationsHandler(port);

        await handler.HandleAsync(cancellationToken);

        Assert.Equal(
            cancellationToken,
            port.ReceivedCancellationToken);
    }

    private sealed class FakeGetOrganizationsPort(
        IReadOnlyList<GetOrganizationsResponse> organizations)
        : IGetOrganizationsPort
    {
        public CancellationToken ReceivedCancellationToken { get; private set; }

        public Task<IReadOnlyList<GetOrganizationsResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            ReceivedCancellationToken = cancellationToken;

            return Task.FromResult(organizations);
        }
    }
}
