using ServerEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Killfeed;

internal struct KillInteraction
{
    public ClientIdentifier Killer { get; init; }
    public ClientIdentifier Victim { get; init; }
}
