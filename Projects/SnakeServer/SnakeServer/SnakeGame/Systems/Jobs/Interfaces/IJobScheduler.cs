using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Jobs.Interfaces;

internal interface IJobScheduler
{
    public void Start(object key, IBackgroundTask job);
    public void Stop(object key);
}
