using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.Killfeed.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Killfeed;

internal static class StartUp
{
    public static void AddKillfeed(this IServiceCollection services)
    {
        services.AddSingleton<IEventListener<KillInteraction>, KillFeedManager>();
        services.AddSingleton<ISessionService, LoginNotificationManager>();
        services.AggregateEvents<KillInteraction>();
    }
}
