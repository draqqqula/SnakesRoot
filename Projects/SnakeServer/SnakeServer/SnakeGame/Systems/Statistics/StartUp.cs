using Microsoft.Extensions.DependencyInjection;
using SnakeGame.Systems.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Statistics;

internal static class StartUp
{
    public static void AddStatistics(this IServiceCollection services)
    {
        services.AddSingleton<IStatisticsFactory, StatisticsFactory>();
    }
}
