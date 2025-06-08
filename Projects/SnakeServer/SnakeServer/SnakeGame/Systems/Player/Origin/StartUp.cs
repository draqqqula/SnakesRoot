using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.Killfeed;
using SnakeGame.Systems.Respawn;
using SnakeGame.Systems.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Player.Origin;

internal static class StartUp
{
    public static void AddPlayerOrigin(this IServiceCollection services)
    {
        services.AddSyncronizedDictionary<Guid, int>("origins");
        services.AddSingleton<OriginAssigner>();
        services.AddSingleton<IEventListener<KillInteraction>>(provider => provider.GetRequiredService<OriginAssigner>());
        services.AddSingleton<IEventListener<RespawnEventArgs>>(provider => provider.GetRequiredService<OriginAssigner>());
        services.AddSingleton<ISessionService>(provider => provider.GetRequiredService<OriginAssigner>());
    }
}
