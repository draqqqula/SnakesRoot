using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.EventPipeline;

internal class EventAggregator<T>
    (
    IEnumerable<IEventListener<T>> listeners,
    IEnumerable<IEventSource<T>> sources
    ) : IStartUpService
{
    public void Start(IGameContext context)
    {
        foreach (var source in sources)
        {
            foreach (var listener in listeners)
            {
                source.Fire += listener.OnFired;
            }
        }
    }
}
