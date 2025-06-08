using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine;

public static class StartUp
{
    public static void AddGameApplication(this IServiceCollection services)
    {
        services.AddSingleton<ISessionStorage<Guid>, SessionStorage>();
        services.AddScoped<IGameApplication, GameApplication>();
    }
}
