using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGame.Systems.Digging.Interfaces;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

public static class StartUp
{
    public static void AddDigging(this IServiceCollection services)
    {
        services.AddSingleton<MapProvider>();
        services.AddSingleton<IMapProvider>(provider => provider.GetRequiredService<MapProvider>());
        services.AddSingleton<ISessionService>(provider => provider.GetRequiredService<MapProvider>());
        services.AddSingleton<IStartUpService>(provider => provider.GetRequiredService<MapProvider>());

        services.AddSingleton<GridProvider>();
        services.AddSingleton<IGridProvider>(provider => provider.GetRequiredService<GridProvider>());
        services.AddSingleton<IStartUpService>(provider => provider.GetRequiredService<GridProvider>());

        services.AddSingleton<Terrain>();

        services.AddSingleton<MapPictureRepository>();
        services.AddSingleton<ISessionService>(provider => provider.GetRequiredService<MapPictureRepository>());

        services.AddSingleton<TerrainDataSender>();

        services.AddSingleton<DiggingManager>();
        services.AddSingleton<IUpdateService>(provider => provider.GetRequiredService<DiggingManager>());
        services.AddSingleton<IEventListener<RespawnEventArgs>>(provider => provider.GetRequiredService<DiggingManager>());
    }
}
