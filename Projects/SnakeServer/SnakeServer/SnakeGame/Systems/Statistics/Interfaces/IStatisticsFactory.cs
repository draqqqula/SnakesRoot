using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Statistics.Interfaces;

internal interface IStatisticsFactory
{
    public IStatistic<T> Declare<T>(string name, T defaultValue);
}
