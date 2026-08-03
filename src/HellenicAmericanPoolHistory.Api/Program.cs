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
using HellenicAmericanPoolHistory.Application.Features.Tournaments.UpdateTournament;
using HellenicAmericanPoolHistory.Application.Features.Tournaments.DeleteTournament;
using HellenicAmericanPoolHistory.Application.Features.Venues.UpdateVenue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreatePlayerHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateVenueRequestValidator>();
builder.Services.AddScoped<CreatePlayerHandler>();
builder.Services.AddScoped<GetPlayerHandler>();
builder.Services.AddScoped<GetPlayersHandler>();
builder.Services.AddScoped<UpdatePlayerHandler>();
builder.Services.AddScoped<DeletePlayerHandler>();
builder.Services.AddScoped<CreateTournamentHandler>();
builder.Services.AddScoped<CreateVenueHandler>();
builder.Services.AddScoped<GetVenueHandler>();
builder.Services.AddScoped<GetVenuesHandler>();
builder.Services.AddScoped<GetTournamentHandler>();
builder.Services.AddScoped<GetTournamentsHandler>();
builder.Services.AddScoped<UpdateTournamentHandler>();
builder.Services.AddScoped<DeleteTournamentHandler>();
builder.Services.AddScoped<UpdateVenueHandler>();

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

app.MapCreateVenueEndpoint();

app.MapGetVenueEndpoint();

app.MapGetVenuesEndpoint();

app.MapGetTournamentEndpoint();

app.MapGetTournamentsEndpoint();

app.MapUpdateTournamentEndpoint();

app.MapDeleteTournamentEndpoint();

app.MapUpdateVenueEndpoint();

app.Run();