using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.NodeBasedCollision.Interfaces;

internal interface IRoundShape
{
    public IEnumerable<Vector2> Points { get; }
}
