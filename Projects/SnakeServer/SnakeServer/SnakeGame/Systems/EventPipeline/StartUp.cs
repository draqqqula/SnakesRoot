using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.EventPipeline;

public static class StartUp
{
    public static void AggregateEvents<T>(this IServiceCollection services)
    {
        services.AddSingleton<IStartUpService, EventAggregator<T>>();
    }
}
