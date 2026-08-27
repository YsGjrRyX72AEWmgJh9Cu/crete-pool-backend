using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Domain.Organization;

namespace HellenicAmericanPoolHistory.Application.Tests.Features.TournamentSeries.GetTournamentSeriesByOrganization;

public sealed class GetTournamentSeriesByOrganizationHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_TournamentSeries_From_Port()
    {
        var organizationId = OrganizationId.New();

        var tournamentSeries =
            new List<GetTournamentSeriesByOrganizationResponse>
            {
                new(
                    Guid.NewGuid(),
                    "Series A",
                    organizationId.Value),

                new(
                    Guid.NewGuid(),
                    "Series B",
                    organizationId.Value)
            };

        var port =
            new FakeGetTournamentSeriesByOrganizationPort(
                tournamentSeries);

        var handler =
            new GetTournamentSeriesByOrganizationHandler(port);

        var result =
            await handler.HandleAsync(
                organizationId,
                CancellationToken.None);

        Assert.Equal(
            tournamentSeries,
            result);

        Assert.Equal(
            organizationId,
            port.ReceivedOrganizationId);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Empty_List_When_Port_Returns_Empty_List()
    {
        var organizationId = OrganizationId.New();

        var port =
            new FakeGetTournamentSeriesByOrganizationPort(
                Array.Empty<GetTournamentSeriesByOrganizationResponse>());

        var handler =
            new GetTournamentSeriesByOrganizationHandler(port);

        var result =
            await handler.HandleAsync(
                organizationId,
                CancellationToken.None);

        Assert.Empty(result);

        Assert.Equal(
            organizationId,
            port.ReceivedOrganizationId);
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Port()
    {
        var organizationId = OrganizationId.New();

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var port =
            new FakeGetTournamentSeriesByOrganizationPort(
                Array.Empty<GetTournamentSeriesByOrganizationResponse>());

        var handler =
            new GetTournamentSeriesByOrganizationHandler(port);

        await handler.HandleAsync(
            organizationId,
            cancellationToken);

        Assert.Equal(
            organizationId,
            port.ReceivedOrganizationId);

        Assert.Equal(
            cancellationToken,
            port.ReceivedCancellationToken);
    }

    private sealed class FakeGetTournamentSeriesByOrganizationPort(
        IReadOnlyList<GetTournamentSeriesByOrganizationResponse> tournamentSeries)
        : IGetTournamentSeriesByOrganizationPort
    {
        public OrganizationId? ReceivedOrganizationId { get; private set; }

        public CancellationToken ReceivedCancellationToken { get; private set; }

        public Task<IReadOnlyList<GetTournamentSeriesByOrganizationResponse>> GetAllAsync(
            OrganizationId organizationId,
            CancellationToken cancellationToken)
        {
            ReceivedOrganizationId = organizationId;
            ReceivedCancellationToken = cancellationToken;

            return Task.FromResult(
                tournamentSeries);
        }
    }
}
