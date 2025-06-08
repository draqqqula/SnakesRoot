using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeGame.Systems.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Jobs;

internal class JobScheduler : IJobScheduler, IUpdateService
{
    private Dictionary<object, IBackgroundTask> _jobs = [];
    public void Start(object key, IBackgroundTask job)
    {
        _jobs[key] = job;
    }

    public void Stop(object key)
    {
        _jobs.Remove(key);
    }

    public void Update(IGameContext context)
    {
        foreach (var pair in _jobs.ToArray())
        {
            if (!pair.Value.Continue(context))
            {
                _jobs.Remove(pair.Key);
            }
        }
    }
}
