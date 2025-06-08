using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Services;
using SnakeGame.Systems.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Jobs;

internal static class StartUp
{
    public static void AddJobScheduler(this IServiceCollection services)
    {
        services.AddSingleton<JobScheduler>();
        services.AddSingleton<IJobScheduler>(provider => provider.GetRequiredService<JobScheduler>());
        services.AddSingleton<IUpdateService>(provider => provider.GetRequiredService<JobScheduler>());
    }
}
