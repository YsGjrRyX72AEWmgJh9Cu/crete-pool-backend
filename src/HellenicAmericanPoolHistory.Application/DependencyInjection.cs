using HellenicAmericanPoolHistory.Application.Features.Players.CreatePlayer;
using Microsoft.Extensions.DependencyInjection;

namespace HellenicAmericanPoolHistory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreatePlayerHandler>();

        return services;
    }
}