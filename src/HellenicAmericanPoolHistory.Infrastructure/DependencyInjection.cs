using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.DeleteParticipation;
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
using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.DeleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.GetParticipations;
using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.GetMatch;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.ScheduleTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.StartTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CompleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.CancelTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GenerateTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.GetMatches;
using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Matches.RecordMatchResult;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Tournaments.GetTournamentBracket;

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
        services.AddScoped<IRecordMatchResultPort, RecordMatchResultPort>();
        services.AddScoped<ICreateParticipationPort, CreateParticipationPort>();
        services.AddScoped<IDeleteParticipationPort, DeleteParticipationPort>();
        services.AddScoped<IGetParticipationPort, GetParticipationPort>();
        services.AddScoped<IUpdateParticipationPort, UpdateParticipationPort>();
        services.AddScoped<IGetParticipationsPort, GetParticipationsPort>();
        services.AddScoped<IGetPlayerPort, GetPlayerPort>();
        services.AddScoped<IGetPlayersPort, GetPlayersPort>();
        services.AddScoped<IUpdatePlayerPort, UpdatePlayerPort>();
        services.AddScoped<IDeletePlayerPort, DeletePlayerPort>();

        services.AddScoped<ICreateTournamentPort, CreateTournamentPort>();

        services.AddScoped<ICreateMatchPort, CreateMatchPort>();

        services.AddScoped<IGetMatchPort, GetMatchPort>();

        services.AddScoped<IGetMatchesPort, GetMatchesPort>();

        services.AddScoped<IDeleteMatchPort, DeleteMatchPort>();

        services.AddScoped<ICreateVenuePort, CreateVenuePort>();

        services.AddScoped<IGetVenuePort, GetVenuePort>();

        services.AddScoped<IGetVenuesPort, GetVenuesPort>();
        services.AddScoped<IUpdateVenuePort, UpdateVenuePort>();
        services.AddScoped<IDeleteVenuePort, DeleteVenuePort>();

        services.AddScoped<IGetTournamentPort, GetTournamentPort>();

        services.AddScoped<IGetTournamentsPort, GetTournamentsPort>();

        services.AddScoped<IUpdateTournamentPort, UpdateTournamentPort>();
        services.AddScoped<IDeleteTournamentPort, DeleteTournamentPort>();
        services.AddScoped<IScheduleTournamentPort, ScheduleTournamentPort>();
        services.AddScoped<IStartTournamentPort, StartTournamentPort>();
        services.AddScoped<ICompleteTournamentPort, CompleteTournamentPort>();
        services.AddScoped<ICancelTournamentPort, CancelTournamentPort>();

        services.AddScoped<
            IGenerateTournamentBracketPort,
            GenerateTournamentBracketPort>();

        services.AddScoped<
            IAdvanceTournamentBracketPort,
            AdvanceTournamentBracketPort>();

        services.AddScoped<
            IGetTournamentBracketPort,
            GetTournamentBracketPort>();

        return services;
    }
}
