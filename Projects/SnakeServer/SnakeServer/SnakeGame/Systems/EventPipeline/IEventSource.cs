using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.EventPipeline;

public interface IEventSource<T>
{
    public event Action<T> Fire;
}
