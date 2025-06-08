using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Statistics;

internal class StatisticsFactory(IRuntimeCommandFactory Factory) : IStatisticsFactory
{

    public IStatistic<T> Declare<T>(string name, T defaultValue)
    {
        var command = new RuntimeCommand<T>(name, Factory);
        var statistic = new PlayerStatistic<T>(command, defaultValue);
        return statistic;
    }
}
