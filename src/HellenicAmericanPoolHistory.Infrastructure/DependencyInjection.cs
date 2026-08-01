using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using HellenicAmericanPoolHistory.Infrastructure.Persistence;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.GetPlayer;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Players.GetPlayers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.GetVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.GetVenues;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournaments;

namespace HellenicAmericanPoolHistory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICreatePlayerPort, CreatePlayerPort>();
        services.AddScoped<IGetPlayerPort, GetPlayerPort>();
        services.AddScoped<IGetPlayersPort, GetPlayersPort>();
        services.AddScoped<IUpdatePlayerPort, UpdatePlayerPort>();
        services.AddScoped<IDeletePlayerPort, DeletePlayerPort>();

        services.AddScoped<ICreateTournamentPort, CreateTournamentPort>();

        services.AddScoped<ICreateVenuePort, CreateVenuePort>();

        services.AddScoped<IGetVenuePort, GetVenuePort>();

        services.AddScoped<IGetVenuesPort, GetVenuesPort>();

        services.AddScoped<IGetTournamentPort, GetTournamentPort>();

        services.AddScoped<IGetTournamentsPort, GetTournamentsPort>();

        return services;
    }
}