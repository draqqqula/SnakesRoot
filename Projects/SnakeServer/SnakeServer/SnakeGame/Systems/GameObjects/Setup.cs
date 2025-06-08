using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGame.Systems.GameObjects.PowerUps;
using SnakeGame.Systems.GameObjects.PowerUps.ScoreDoubler;

namespace SnakeGame.Systems.GameObjects;

internal static class SetUp
{
    public static void AddGameObjects(this IServiceCollection services)
    {
        services.AddSingleton<List<PowerUp>>();
        services.AddSingleton<PowerUpSpawner>();
        services.AddSingleton<IUpdateService>(provider => provider.GetRequiredService<PowerUpSpawner>());
        services.AddSingleton<DoublerSpawner>();
        services.AddSingleton<IStartUpService>(provider => provider.GetRequiredService<DoublerSpawner>());
    }
}
