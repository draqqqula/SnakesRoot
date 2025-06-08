using ServerEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Jobs.Interfaces;

internal interface IBackgroundTask
{
    public bool Continue(IGameContext context);
}
