using Microsoft.Extensions.DependencyInjection;
using ServerEngine;
using ServerEngine.Interfaces;
using SnakeGame.Common;

namespace SnakeGame;

public static class StartUp
{
    public static void AddGameLauncher(this IServiceCollection services)
    {
        services.AddSingleton<ISessionLauncher, GameLauncher>();
    }
}
