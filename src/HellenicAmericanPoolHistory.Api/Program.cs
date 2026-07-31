using HellenicAmericanPoolHistory.Application.Features.Players.DeletePlayer;
using HellenicAmericanPoolHistory.Api.ExceptionHandling;
using HellenicAmericanPoolHistory.Api.Endpoints.Players;
using HellenicAmericanPoolHistory.Application.Features.Players.UpdatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayers;
using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using HellenicAmericanPoolHistory.Application.Features.Players.GetPlayer;
using HellenicAmericanPoolHistory.Infrastructure;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreatePlayerHandler>();

builder.Services.AddScoped<CreatePlayerHandler>();
builder.Services.AddScoped<GetPlayerHandler>();
builder.Services.AddScoped<GetPlayersHandler>();
builder.Services.AddScoped<UpdatePlayerHandler>();
builder.Services.AddScoped<DeletePlayerHandler>();

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

app.Run();