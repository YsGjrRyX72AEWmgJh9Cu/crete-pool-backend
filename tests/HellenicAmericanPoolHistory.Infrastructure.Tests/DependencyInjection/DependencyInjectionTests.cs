using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;
using HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;
using HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;
using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganization;
using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;
using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Infrastructure.Tests.DependencyInjection;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_Should_Return_The_Same_ServiceCollection()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Host=localhost;Database=test;Username=test;Password=test"
                })
            .Build();

        var result = services.AddInfrastructure(configuration);

        Assert.Same(services, result);
    }

    [Fact]
    public void AddInfrastructure_Should_Register_ApplicationDbContext()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Host=localhost;Database=test;Username=test;Password=test"
                })
            .Build();

        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        Assert.NotNull(context);
    }

    [Fact]
    public void AddInfrastructure_Should_Register_All_Ports()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Host=localhost;Database=test;Username=test;Password=test"
                })
            .Build();

        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreatePlayerPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IDeletePlayerPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetPlayerPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetPlayersPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IUpdatePlayerPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreateOrganizationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetOrganizationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetOrganizationsPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreateTournamentSeriesPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetTournamentSeriesPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<
                IGetTournamentSeriesByOrganizationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreateTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetTournamentsPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IUpdateTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IDeleteTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IScheduleTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IStartTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICompleteTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICancelTournamentPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<
                IGenerateTournamentBracketPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<
                IAdvanceTournamentBracketPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<
                IGetTournamentBracketPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreateVenuePort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetVenuePort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetVenuesPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IUpdateVenuePort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IDeleteVenuePort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreateParticipationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IDeleteParticipationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetParticipationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetParticipationsPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IUpdateParticipationPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<ICreateMatchPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetMatchPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IGetMatchesPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<IDeleteMatchPort>());

        Assert.NotNull(
            serviceProvider.GetRequiredService<
                IRecordMatchResultPort>());
    }
}
