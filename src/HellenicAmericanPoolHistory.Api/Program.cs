using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Api.ExceptionHandling;
using HellenicAmericanPoolHistory.Api.Endpoints.Players;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Infrastructure;
using FluentValidation;
using HellenicAmericanPoolHistory.Api.Endpoints.Tournaments;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CreateTournament;
using HellenicAmericanPoolHistory.Api.Endpoints.Venues;
using HellenicAmericanPoolHistory.Application.Features.Venues.CreateVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.GetVenues;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournaments;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeries;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GetTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;
using HellenicAmericanPoolHistory.Application.Features.Venues.DeleteVenue;
using HellenicAmericanPoolHistory.Api.Endpoints.Participations;
using HellenicAmericanPoolHistory.Application.Features.Participations.CreateParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.DeleteParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipation;
using HellenicAmericanPoolHistory.Application.Features.Participations.GetParticipations;
using HellenicAmericanPoolHistory.Application.Features.Organizations.GetOrganizations;
using HellenicAmericanPoolHistory.Application.Features.Participations.UpdateParticipation;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.ScheduleTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.StartTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CompleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.CancelTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.AdvanceTournamentBracket;
using HellenicAmericanPoolHistory.Api.Endpoints.Matches;
using HellenicAmericanPoolHistory.Application.Features.Matches.CreateMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.GetMatches;
using HellenicAmericanPoolHistory.Application.Features.Matches.DeleteMatch;
using HellenicAmericanPoolHistory.Application.Features.Matches.RecordMatchResult;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.GenerateTournamentBracket;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.CreateTournamentSeries;
using HellenicAmericanPoolHistory.Application.Features.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Api.Endpoints.TournamentSeries.GetTournamentSeriesByOrganization;
using HellenicAmericanPoolHistory.Api.Endpoints.TournamentSeries;
using HellenicAmericanPoolHistory.Application.Features.Organizations.CreateOrganization;
using HellenicAmericanPoolHistory.Api.Endpoints.Organizations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateTournamentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateParticipationRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateVenueRequestValidator>();
builder.Services.AddScoped<CreatePlayerHandler>();
builder.Services.AddScoped<GetPlayerHandler>();
builder.Services.AddScoped<GetPlayersHandler>();
builder.Services.AddScoped<UpdatePlayerHandler>();
builder.Services.AddScoped<DeletePlayerHandler>();
builder.Services.AddScoped<CreateTournamentHandler>();
builder.Services.AddScoped<CreateOrganizationHandler>();
builder.Services.AddScoped<CreateTournamentSeriesHandler>();
builder.Services.AddScoped<GetOrganizationsHandler>();
builder.Services.AddScoped<CreateVenueHandler>();
builder.Services.AddScoped<GetVenueHandler>();
builder.Services.AddScoped<GetVenuesHandler>();
builder.Services.AddScoped<GetTournamentHandler>();
builder.Services.AddScoped<GetTournamentsHandler>();
builder.Services.AddScoped<GetTournamentSeriesHandler>();
builder.Services.AddScoped<GetTournamentSeriesByOrganizationHandler>();
builder.Services.AddScoped<UpdateTournamentHandler>();
builder.Services.AddScoped<DeleteTournamentHandler>();
builder.Services.AddScoped<ScheduleTournamentHandler>();
builder.Services.AddScoped<StartTournamentHandler>();
builder.Services.AddScoped<ScheduleTournamentHandler>();
builder.Services.AddScoped<CompleteTournamentHandler>();
builder.Services.AddScoped<StartTournamentHandler>();
builder.Services.AddScoped<CancelTournamentHandler>();
builder.Services.AddScoped<UpdateVenueHandler>();
builder.Services.AddScoped<DeleteVenueHandler>();
builder.Services.AddScoped<CreateParticipationHandler>();
builder.Services.AddScoped<CreateMatchHandler>();
builder.Services.AddScoped<GetMatchHandler>();
builder.Services.AddScoped<GetMatchesHandler>();
builder.Services.AddScoped<DeleteMatchHandler>();
builder.Services.AddScoped<RecordMatchResultHandler>();
builder.Services.AddScoped<DeleteParticipationHandler>();
builder.Services.AddScoped<GetParticipationHandler>();
builder.Services.AddScoped<GetParticipationsHandler>();
builder.Services.AddScoped<UpdateParticipationHandler>();
builder.Services.AddScoped<GenerateTournamentBracketHandler>();
builder.Services.AddScoped<GetTournamentBracketHandler>();
builder.Services.AddScoped<AdvanceTournamentBracketHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCreatePlayerEndpoint();

app.MapGetPlayerEndpoint();

app.MapGetPlayersEndpoint();

app.MapUpdatePlayerEndpoint();

app.MapDeletePlayerEndpoint();

app.MapCreateTournamentEndpoint();

app.MapCreateTournamentSeriesEndpoint();

app.MapCreateVenueEndpoint();

app.MapGetVenueEndpoint();

app.MapGetVenuesEndpoint();

app.MapGetTournamentEndpoint();

app.MapGetTournamentBracketEndpoint();

app.MapCompleteTournamentEndpoint();

app.MapCancelTournamentEndpoint();

app.MapGetTournamentsEndpoint();

app.MapUpdateTournamentEndpoint();

app.MapDeleteTournamentEndpoint();

app.MapScheduleTournamentEndpoint();

app.MapStartTournamentEndpoint();

app.MapGenerateTournamentBracketEndpoint();

app.MapAdvanceTournamentBracketEndpoint();

app.MapUpdateVenueEndpoint();

app.MapDeleteVenueEndpoint();

app.MapCreateParticipationEndpoint();

app.MapCreateMatchEndpoint();

app.MapCreateOrganizationEndpoint();

app.MapGetOrganizationsEndpoint();

app.MapGetTournamentSeriesEndpoint();

app.MapGetTournamentSeriesByOrganizationEndpoint();

app.MapRecordMatchResultEndpoint();

app.MapGetMatchEndpoint();

app.MapGetMatchesEndpoint();

app.MapDeleteMatchEndpoint();

app.MapDeleteParticipationEndpoint();

app.MapGetParticipationEndpoint();

app.MapGetParticipationsEndpoint();

app.MapUpdateParticipationEndpoint();

app.Run();

public partial class Program
{
}
